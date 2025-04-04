using Dapper;
using RChat.Domain.Members;
using RChat.Domain.Messages;
using RChat.Domain.Messages.Repository;
using RChat.Domain.Users;
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
                 m.updated_at AS {nameof(Message.UpdatedAt)},
                 m.deleted_at AS {nameof(Message.DeletedAt)},
                 u.id AS {nameof(Message.Sender.Id)},
                 u.account_id AS {nameof(Message.Sender.AccountId)},
                 u.username AS {nameof(Message.Sender.Username)},
                 u.firstname AS {nameof(Message.Sender.Firstname)},
                 u.lastname AS {nameof(Message.Sender.Lastname)},
                 u.description AS {nameof(Message.Sender.Description)},
                 u.date_of_birth AS {nameof(Message.Sender.DateOfBirth)},
                 u.created_at AS {nameof(Message.Sender.CreatedAt)},
                 u.updated_at AS {nameof(Message.Sender.UpdatedAt)}
             FROM public.message AS m
             JOIN public.user AS u ON u.id = m.sender_id
             """;

        using var connection = await connectionFactory.CreateAsync();

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);

        if (parameters.OnlyActive is not null)
        {
            queryBuilder.AddCondition("m.deleted_at IS NULL");
        }
        
        if (parameters.Pagination is not null)
        {
            queryBuilder.AddPagination(parameters.Pagination);
        }
        
        if (parameters.Sorting is not null)
        {
            var sortingColumnDbMapping = new Dictionary<MessageSortingColumn, string>
            {
                { MessageSortingColumn.CreatedAt, "m.created_at" }
            };
            
            queryBuilder.AddSorting(sortingColumnDbMapping, parameters.Sorting);
        }

        if (parameters.ChatId is not null)
        {
            queryBuilder.AddCondition("m.chat_id = @ChatId");
            queryBuilder.AddParameter("@ChatId", parameters.ChatId.Value);
        }

        var messages = await connection.QueryAsync<Message, User, Message>(
            queryBuilder.BuildQuery(),
            (message, user) =>
            {
                message.Sender = user;
                
                return message;
            },
        queryBuilder.GetParameters(),
            splitOn:$"{nameof(Message.Id)}, {nameof(Message.Sender.Id)}");

        return messages.ToList();
    }

    public async Task<List<Message>> GetLatestMessagesByChatIdsAsync(int[] chatIds)
    {
        const string sql =
            $"""
             SELECT DISTINCT ON (m.chat_id)
                 m.id AS {nameof(Member.Id)},
                 m.text AS {nameof(Message.Text)},
                 m.chat_id AS {nameof(Message.ChatId)},
                 m.sender_id AS {nameof(Message.SenderId)},
                 m.reply_to_message_id AS {nameof(Message.ReplyToMessageId)},
                 m.created_at AS {nameof(Message.CreatedAt)},
                 m.updated_at AS {nameof(Message.UpdatedAt)},
                 m.deleted_at AS {nameof(Message.DeletedAt)},
                 u.id AS {nameof(Message.Sender.Id)},
                 u.account_id AS {nameof(Message.Sender.AccountId)},
                 u.username AS {nameof(Message.Sender.Username)},
                 u.firstname AS {nameof(Message.Sender.Firstname)},
                 u.lastname AS {nameof(Message.Sender.Lastname)},
                 u.description AS {nameof(Message.Sender.Description)},
                 u.date_of_birth AS {nameof(Message.Sender.DateOfBirth)},
                 u.created_at AS {nameof(Message.Sender.CreatedAt)},
                 u.updated_at AS {nameof(Message.Sender.UpdatedAt)}
             FROM public.message AS m
             JOIN public.user AS u ON u.id = m.sender_id
             WHERE m.chat_id = ANY(@ChatIds) AND m.deleted_at IS NULL
             ORDER BY m.chat_id, m.created_at DESC;
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        var messages = await connection.QueryAsync<Message, User, Message>(
            sql,
            (message, user) =>
            {
                message.Sender = user;
                
                return message;
            },
            new { ChatIds = chatIds},
            splitOn:$"{nameof(Message.Id)}, {nameof(Message.Sender.Id)}");

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
             deleted_at = @{nameof(Message.DeletedAt)},
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