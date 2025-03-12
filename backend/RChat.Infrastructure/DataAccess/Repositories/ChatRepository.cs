using System.Text;
using Dapper;
using RChat.Domain.Chats;
using RChat.Domain.Chats.Repository;
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
            param: new { ChatId = chatId });

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
                 cg.chat_id AS {nameof(ChatGroup.ChatId)},
                 cg.name AS {nameof(ChatGroup.Name)},
                 cg.description AS {nameof(ChatGroup.Description)},
                 cg.is_private AS {nameof(ChatGroup.IsPrivate)}
             FROM public.chat AS c
             LEFT JOIN public.chat_group AS cg ON cg.chat_id = c.id
             """;
        
        QueryPagination? pagination = parameters.Pagination is not null
            ? new QueryPagination(parameters.Pagination.Skip, parameters.Pagination.Take)
            : null;

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql, pagination);
        
        using var connection = await connectionFactory.CreateAsync();
        
        var chats = await connection.QueryAsync<Chat, ChatGroup, Chat>(
            queryBuilder.BuildQuery(),
            (chat, chatGroup) =>
            {
                chat.GroupChat = chatGroup;
                return chat;
            },
            param: queryBuilder.GetParameters());

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
        if (chat.GroupChat is null)
            return;
        
        const string updateChatGroupSql =
            $"""
             UPDATE public.chat_group
             SET name = @{nameof(chat.GroupChat.Name)}, 
             description = @{nameof(chat.GroupChat.Description)}, 
             is_private = @{nameof(chat.GroupChat.IsPrivate)}
             WHERE chat_id = @{nameof(chat.GroupChat.ChatId)};
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        await connection.ExecuteAsync(updateChatGroupSql, chat.GroupChat);
    }
}