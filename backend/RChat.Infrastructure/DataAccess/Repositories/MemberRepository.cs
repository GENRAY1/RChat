using Dapper;
using RChat.Domain.Members;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.QueryBuilders;

namespace RChat.Infrastructure.DataAccess.Repositories;

public class MemberRepository(IDbConnectionFactory connectionFactory) : IMemberRepository
{
    public async Task<Member?> GetByIdAsync(int memberId)
    {
        const string defaultSql =
            $"""
             SELECT 
                 m.id AS {nameof(Member.Id)},
                 m.user_id AS {nameof(Member.UserId)},
                 m.chat_id AS {nameof(Member.ChatId)},
                 m.joined_at AS {nameof(Member.JoinedAt)}
             FROM public.member AS m
             WHERE m.id = @MemberId
             """;

        using var connection = await connectionFactory.CreateAsync();
        
        return await connection.QueryFirstOrDefaultAsync<Member>(defaultSql, new { MemberId = memberId });
    }

    public async Task<List<Member>> GetListAsync(GetMemberListParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 m.id AS {nameof(Member.Id)},
                 m.user_id AS {nameof(Member.UserId)},
                 m.chat_id AS {nameof(Member.ChatId)},
                 m.joined_at AS {nameof(Member.JoinedAt)}
             FROM public.member AS m
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        QueryPagination? pagination = parameters.Pagination is not null
            ? new QueryPagination(parameters.Pagination.Skip, parameters.Pagination.Take)
            : null;

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql, pagination);
        
        var members = await connection.QueryAsync<Member>(
            queryBuilder.BuildQuery(),
            param: queryBuilder.GetParameters());
        
        return members.ToList();
    }

    public async Task<int> CreateAsync(Member member)
    {
        const string defaultSql =
            $"""
             INSERT INTO public.member
             (user_id, chat_id, joined_at)
             VALUES (
                @{nameof(Member.UserId)},
                @{nameof(Member.ChatId)}, 
                @{nameof(Member.JoinedAt)}
             )
             RETURNING id;
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        var memberId = await connection.ExecuteScalarAsync<int>(defaultSql, member);
        
        return memberId;
    }

    public async Task DeleteAsync(int memberId)
    {
        const string defaultSql =
            """
            DELETE FROM public.member
            WHERE id = @MemberId;
            """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        await connection.ExecuteAsync(defaultSql,  new {MemberId = memberId});
    }
}