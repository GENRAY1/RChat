using Dapper;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.QueryBuilders;

namespace RChat.Infrastructure.DataAccess.Repositories;

public class ChatRepository(IDbConnectionFactory connectionFactory)
    : IChatRepository
{
    public async Task<Chat?> GetByIdAsync(int chatId)
    {
        const string defaultSql =
            $"""
             SELECT 
                 c.id AS {nameof(Chat.Id)},
                 c.type AS {nameof(Chat.Type)},
                 c.creator_id AS {nameof(Chat.CreatorId)},
                 c.created_at AS {nameof(Chat.CreatedAt)},
                 c.deleted_at AS {nameof(Chat.DeletedAt)},
                 cg.chat_id AS {nameof(ChatGroup.ChatId)},
                 cg.name AS {nameof(ChatGroup.Name)},
                 cg.description AS {nameof(ChatGroup.Description)},
                 cg.is_private AS {nameof(ChatGroup.IsPrivate)}
             FROM public.chat AS c
             LEFT JOIN public.chat_group AS cg ON cg.chat_id = c.id
             WHERE c.id = @ChatId
             """;

        using var connection = await connectionFactory.CreateAsync();

        var chats = await connection.QueryAsync<Chat, ChatGroup, Chat>(
            defaultSql,
            (chat, chatGroup) =>
            {
                chat.GroupChat = chatGroup;
                return chat;
            },
            param: new { ChatId = chatId },
            splitOn: $"{nameof(Chat.Id)}, {nameof(ChatGroup.ChatId)}");

        return chats.FirstOrDefault();
    }

    public async Task<List<Chat>> GetListAsync(GetChatListParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 c.id AS {nameof(Chat.Id)},
                 c.type AS {nameof(Chat.Type)},
                 c.creator_id AS {nameof(Chat.CreatorId)},
                 c.created_at AS {nameof(Chat.CreatedAt)},
                 c.deleted_at AS {nameof(Chat.DeletedAt)},
                 cg.chat_id AS {nameof(ChatGroup.ChatId)},
                 cg.name AS {nameof(ChatGroup.Name)},
                 cg.description AS {nameof(ChatGroup.Description)},
                 cg.is_private AS {nameof(ChatGroup.IsPrivate)}
             FROM public.chat AS c
             LEFT JOIN public.chat_group AS cg ON cg.chat_id = c.id
             """;

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);

        if (parameters.Pagination is not null)
        {
            queryBuilder.AddPagination(parameters.Pagination);
        }
        
        if (parameters.Sorting is not null)
        {
            var sortingColumnDbMapping = new Dictionary<ChatSortingColumn, string>
            {
                { ChatSortingColumn.CreatedAt, "c.created_at" },
                { ChatSortingColumn.DeletedAt, "c.deleted_at" },
                { ChatSortingColumn.Type, "c.type" },
                { ChatSortingColumn.GroupName, "cg.name" },
                { ChatSortingColumn.GroupIsPrivate, "cg.is_private" }
            };
            
            queryBuilder.AddSorting(sortingColumnDbMapping, parameters.Sorting);
        }
        
        bool needIncludeMembers =
            parameters.UserIds is not null 
            || parameters.OnlyAccessibleByUserId is not null;
        
        if (needIncludeMembers) 
            queryBuilder.AddJoin("JOIN public.member AS m ON c.id = m.chat_id");
        
        if (parameters.UserIds is not null)
        {
            queryBuilder.AddCondition("m.user_id = ANY(@UserIds)");
            queryBuilder.AddParameter("@UserIds", parameters.UserIds);
        }
            
        if(parameters.OnlyAccessibleByUserId is not null)
        {
            queryBuilder.AddCondition("(cg.is_private = FALSE OR m.user_id = @UserId)");
            queryBuilder.AddParameter("@UserId", parameters.OnlyAccessibleByUserId);
        }

        if (parameters.OnlyActive is true)
        {
            queryBuilder.AddCondition("c.deleted_at IS NULL");
        }
        
        if (parameters.ChatIds is not null)
        {
            queryBuilder.AddCondition("c.id = ANY(@ChatIds)");
            queryBuilder.AddParameter("@ChatIds", parameters.ChatIds);
        }

        if (parameters.Type is not null)
        {
            queryBuilder.AddCondition("c.type = @Type");
            queryBuilder.AddParameter("@Type", parameters.Type);
        }
        
        using var connection = await connectionFactory.CreateAsync();
        
        var chats = await connection.QueryAsync<Chat, ChatGroup, Chat>(
            queryBuilder.BuildQuery(),
            (chat, chatGroup) =>
            {
                chat.GroupChat = chatGroup;
                return chat;
            },
            param: queryBuilder.GetParameters(),
            splitOn: $"{nameof(Chat.Id)}, {nameof(ChatGroup.ChatId)}");

        return chats.ToList();
    }

    public async Task<int> CreateAsync(Chat chat)
    {
        const string createChatSql =
            $"""
             INSERT INTO public.chat
             (type, creator_id, created_at)
             VALUES (
                @{nameof(Chat.Type)},
                @{nameof(Chat.CreatorId)}, 
                @{nameof(Chat.CreatedAt)}
             )
             RETURNING id;
             """;

        const string createChatGroupSql =
            """
            INSERT INTO public.chat_group
            (chat_id, name, description, is_private)
            VALUES (@ChatId, @Name, @Description, @IsPrivate);
            """;
        
        using var connection = await connectionFactory.CreateAsync();
        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            int chatId = await connection.ExecuteScalarAsync<int>(createChatSql, chat, transaction);
        
            if (chat.GroupChat is not null)
            {
                await connection.ExecuteAsync(createChatGroupSql, param:new
                {
                    ChatId = chatId,
                    chat.GroupChat.Name,
                    chat.GroupChat.Description,
                    chat.GroupChat.IsPrivate
                }, transaction);
            }
            await transaction.CommitAsync();

            return chatId;
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task UpdateAsync(Chat chat)
    {
        const string updateChatSql =
            $"""
             UPDATE public.chat
             SET deleted_at = @{nameof(chat.DeletedAt)}
             WHERE id = @{nameof(chat.Id)};
             """;
        
        const string updateChatGroupSql =
            $"""
             UPDATE public.chat_group
             SET name = @{nameof(chat.GroupChat.Name)}, 
             description = @{nameof(chat.GroupChat.Description)}, 
             is_private = @{nameof(chat.GroupChat.IsPrivate)}
             WHERE chat_id = @{nameof(chat.GroupChat.ChatId)};
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        using var transaction = await connection.BeginTransactionAsync();
        
        try
        {
            await connection.ExecuteAsync(updateChatSql, chat, transaction);
        
            if (chat.GroupChat is not null)
                await connection.ExecuteAsync(updateChatGroupSql, chat.GroupChat, transaction);
            
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}