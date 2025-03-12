using Dapper;
using RChat.Domain.Members;
using RChat.Domain.Messages;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.QueryBuilders;

namespace RChat.Infrastructure.DataAccess.Repositories;

public class MessageRepository(IDbConnectionFactory connectionFactory)
    : IMessageRepository
{
    public async Task<Message?> GetByIdAsync(int messageId)
    {
        const string defaultSql =
            $"""
             SELECT 
                 m.id AS {nameof(Member.Id)},
                 m.text AS {nameof(Message.Text)},
                 m.chat_id AS {nameof(Message.ChatId)},
                 m.sender_id AS {nameof(Message.SenderId)},
                 m.reply_to_message_id AS {nameof(Message.ReplyToMessageId)},
                 m.created_at AS {nameof(Message.CreatedAt)},
                 m.updated_at AS {nameof(Message.UpdatedAt)}
             FROM public.message AS m
             WHERE m.id = @MessageId
             """;

        using var connection = await connectionFactory.CreateAsync();

        return await connection.QueryFirstOrDefaultAsync<Message>(defaultSql, new { MessageId = messageId });
    }

    public async Task<List<Message>> GetListAsync(GetMessageListParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 m.id AS {nameof(Member.Id)},
                 m.text AS {nameof(Message.Text)},
                 m.chat_id AS {nameof(Message.ChatId)},
                 m.sender_id AS {nameof(Message.SenderId)},
                 m.reply_to_message_id AS {nameof(Message.ReplyToMessageId)},
                 m.created_at AS {nameof(Message.CreatedAt)},
                 m.updated_at AS {nameof(Message.UpdatedAt)}
             FROM public.message AS m
             """;

        using var connection = await connectionFactory.CreateAsync();

        QueryPagination? pagination = parameters.Pagination is not null
            ? new QueryPagination(parameters.Pagination.Skip, parameters.Pagination.Take)
            : null;

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql, pagination);

        var messages = await connection.QueryAsync<Message>(
            queryBuilder.BuildQuery(),
            queryBuilder.GetParameters());

        return messages.ToList();
    }

    public async Task<int> CreateAsync(Message message)
    {
        const string defaultSql =
            $"""
             INSERT INTO public.message
             (text, chat_id, sender_id, reply_to_message_id, created_at)
             VALUES (
                @{nameof(Message.Text)},
                @{nameof(Message.ChatId)}, 
                @{nameof(Message.SenderId)},
                @{nameof(Message.ReplyToMessageId)},
                @{nameof(Message.CreatedAt)}
             )
             RETURNING id;
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        var messageId = await connection.ExecuteScalarAsync<int>(defaultSql, param: message);
        
        return messageId;
    }

    public async Task UpdateAsync(Message message)
    {
        const string defaultSql =
            $"""
             UPDATE public.message
             SET text = @{nameof(Message.Text)}, 
             updated_at = @{nameof(Message.UpdatedAt)},
             reply_to_message_id = @{nameof(Message.ReplyToMessageId)}
             WHERE id = @{nameof(Message.Id)};
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        await connection.ExecuteAsync(defaultSql, param: message);
    }

    public async Task DeleteAsync(int messageId)
    {
        const string defaultSql =
            """
            DELETE FROM public.message
            WHERE id = @MessageId;
            """;

        using var connection = await connectionFactory.CreateAsync();

        await connection.ExecuteAsync(defaultSql, new { MessageId = messageId });
    }
}