using Dapper;
using RChat.Domain.Members;
using RChat.Domain.Users;
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
                 m.joined_at AS {nameof(Member.JoinedAt)},
                 u.id AS {nameof(Member.User.Id)},
                 u.username AS {nameof(Member.User.Username)},
                 u.login AS {nameof(Member.User.Login)},
                 u.password AS {nameof(Member.User.Password)},
                 u.description AS {nameof(Member.User.Description)},
                 u.date_of_birth AS {nameof(Member.User.DateOfBirth)},
                 u.created_at AS {nameof(Member.User.CreatedAt)},
                 u.updated_at AS {nameof(Member.User.UpdatedAt)},
                 u.role_id AS {nameof(Member.User.RoleId)}
             FROM public.member AS m
             JOIN public.user AS u ON m.user_id = u.id
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        QueryPagination? pagination = parameters.Pagination is not null
            ? new QueryPagination(parameters.Pagination.Skip, parameters.Pagination.Take)
            : null;

        QueryBuilder queryBuilder = new QueryBuilder(defaultSql, pagination);
        
        if(parameters.ChatIds is not null)
        {
            queryBuilder.AddCondition("m.chat_id=ANY(@ChatIds)");
            queryBuilder.AddParameter("@ChatIds", parameters.ChatIds);
        }
        
        if(parameters.UserIds is not null)
        {
            queryBuilder.AddCondition("m.user_id=ANY(@UserIds)");
            queryBuilder.AddParameter("@UserIds", parameters.UserIds);
        }
        
        string sql = queryBuilder.BuildQuery();

        var members = await connection.QueryAsync<Member, User, Member>(
            sql,
            (member, user) =>
            {
                member.User = user;
                return member;
            },
            param: queryBuilder.GetParameters(),
            splitOn: $"{nameof(Member.Id)}, {nameof(Member.User.Id)}"
        );
        
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
    
    public async Task<int[]> CreateAsync(IEnumerable<Member> members)
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
        using var transaction = await connection.BeginTransactionAsync();

        try
        {
            var memberIds = new List<int>();

            foreach (var member in members)
            {
                var memberId = await connection.ExecuteScalarAsync<int>(defaultSql, member, transaction);
                memberIds.Add(memberId);
            }

            await transaction.CommitAsync();

            return memberIds.ToArray();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
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