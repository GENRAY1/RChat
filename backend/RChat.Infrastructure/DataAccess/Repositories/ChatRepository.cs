using Dapper;
using RChat.Application.Common.Sorting;
using RChat.Application.Dtos.Chats;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
using RChat.Domain.Messages;
using RChat.Domain.Users;
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
                 c.member_count AS {nameof(Chat.MemberCount)},
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
                 c.member_count AS {nameof(Chat.MemberCount)},
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

    public async Task<List<UserChat>> GetUserChatsAsync(GetUserChatsParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 c.id AS {nameof(UserChat.Id)},
                 c.type AS {nameof(UserChat.Type)},
                 c.creator_id AS {nameof(UserChat.CreatorId)},
                 c.deleted_at AS {nameof(UserChat.DeletedAt)},
                 c.created_at AS {nameof(UserChat.CreatedAt)},
                 c.member_count AS {nameof(UserChat.MemberCount)},
                 cg.chat_id AS {nameof(UserChat.GroupChat.ChatId)},
                 cg.name AS {nameof(UserChat.GroupChat.Name)},
                 cg.description AS {nameof(UserChat.GroupChat.Description)},
                 cg.is_private AS {nameof(UserChat.GroupChat.IsPrivate)},
                 lm.id AS {nameof(UserChat.LatestMessage.Id)},
                 lm.text AS {nameof(UserChat.LatestMessage.Text)},
                 lm.chat_id AS {nameof(UserChat.LatestMessage.ChatId)},
                 lm.sender_id AS {nameof(UserChat.LatestMessage.SenderId)},
                 lm.reply_to_message_id AS {nameof(UserChat.LatestMessage.ReplyToMessageId)},
                 lm.created_at AS {nameof(UserChat.LatestMessage.CreatedAt)},
                 lm.updated_at AS {nameof(UserChat.LatestMessage.UpdatedAt)},
                 lm.deleted_at AS {nameof(UserChat.LatestMessage.DeletedAt)},
                 lmu.id AS {nameof(UserChat.LatestMessage.Sender.Id)},
                 lmu.account_id AS {nameof(UserChat.LatestMessage.Sender.AccountId)},
                 lmu.username AS {nameof(UserChat.LatestMessage.Sender.Username)},
                 lmu.firstname AS {nameof(UserChat.LatestMessage.Sender.Firstname)},
                 lmu.lastname AS {nameof(UserChat.LatestMessage.Sender.Lastname)},
                 lmu.description AS {nameof(UserChat.LatestMessage.Sender.Description)},
                 lmu.date_of_birth AS {nameof(UserChat.LatestMessage.Sender.DateOfBirth)},
                 lmu.created_at AS {nameof(UserChat.LatestMessage.Sender.CreatedAt)},
                 lmu.updated_at AS {nameof(UserChat.LatestMessage.Sender.UpdatedAt)}
             FROM public.chat AS c
             JOIN member AS m1 ON m1.chat_id = c.id AND m1.user_id = @UserId
             LEFT JOIN public.chat_group AS cg ON cg.chat_id = c.id
             LEFT JOIN LATERAL (
                 SELECT m.* 
                 FROM public.message AS m
                 WHERE m.chat_id = c.id AND m.deleted_at IS NULL
                 ORDER BY m.created_at DESC 
                 LIMIT 1
             ) AS lm ON TRUE
             LEFT JOIN public.user AS lmu ON lmu.id = lm.sender_id
             """;

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);
        
        queryBuilder.AddCondition("c.deleted_at IS NULL");
        queryBuilder.AddParameter("@UserId", parameters.UserId);
        queryBuilder.AddSorting("COALESCE(lm.created_at, c.created_at)", SortingDirection.Desc);
        
        if (parameters.Pagination is not null)
        {
            queryBuilder.AddPagination(parameters.Pagination);
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

        if (parameters.SecondUserId is not null)
        {
            queryBuilder.AddJoin("JOIN member AS m2 ON m2.chat_id = c.id AND m2.user_id = @SecondUserId");
            queryBuilder.AddParameter("@SecondUserId", parameters.SecondUserId.Value);
        }
        
        using var connection = await connectionFactory.CreateAsync();
        
        string[] splitOn =
        [
            nameof(UserChat.Id),
            nameof(UserChat.GroupChat.ChatId),
            nameof(UserChat.LatestMessage.Id),
            nameof(UserChat.LatestMessage.Sender.Id)
        ];
         
        var chats = await connection.QueryAsync<UserChat, ChatGroup, Message?, User?, UserChat>(
            queryBuilder.BuildQuery(),
            (chat, chatGroup, message, user) =>
            {
                chat.GroupChat = chatGroup;

                if (message is not null)
                {
                    message.Sender = user!;
                    chat.LatestMessage = message;
                }
                
                return chat;
            },
            param: queryBuilder.GetParameters(),
            splitOn: string.Join(",", splitOn));

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