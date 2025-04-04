using Dapper;
using RChat.Domain.Members;
using RChat.Domain.Members.Repository;
using RChat.Domain.Users;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.QueryBuilders;

namespace RChat.Infrastructure.DataAccess.Repositories;

public class MemberRepository(IDbConnectionFactory connectionFactory) : IMemberRepository
{
    public async Task<Member?> GetAsync(GetMemberParameters parameters)
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
        
        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);

        if (parameters.Id.HasValue)
        {
            queryBuilder.AddCondition("m.id = @Id");
            queryBuilder.AddParameter("@Id", parameters.Id.Value);
        }
        
        if (parameters.UserId.HasValue)
        {
            queryBuilder.AddCondition("m.user_id = @UserId");
            queryBuilder.AddParameter("@UserId", parameters.UserId.Value);
        }
        
        if (parameters.ChatId.HasValue)
        {
            queryBuilder.AddCondition("m.chat_id = @ChatId");
            queryBuilder.AddParameter("@ChatId", parameters.ChatId.Value);
        }
        
        return await connection.QueryFirstOrDefaultAsync<Member>(queryBuilder.BuildQuery(), queryBuilder.GetParameters());
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
                 u.account_id AS {nameof(Member.User.AccountId)},
                 u.username AS {nameof(Member.User.Username)},
                 u.firstname AS {nameof(Member.User.Firstname)},
                 u.lastname AS {nameof(Member.User.Lastname)},
                 u.description AS {nameof(Member.User.Description)},
                 u.date_of_birth AS {nameof(Member.User.DateOfBirth)},
                 u.created_at AS {nameof(Member.User.CreatedAt)},
                 u.updated_at AS {nameof(Member.User.UpdatedAt)}
             FROM public.member AS m
             JOIN public.user AS u ON m.user_id = u.id
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);

        if (parameters.Pagination is not null)
        {
            queryBuilder.AddPagination(parameters.Pagination);
        }

        if (parameters.Sorting is not null)
        {
            var sortingColumnDbMapping = new Dictionary<MemberSortingColumn, string>
            {
                { MemberSortingColumn.JoinedAt, "m.joined_at" }
            };
            
            queryBuilder.AddSorting(sortingColumnDbMapping, parameters.Sorting);
        }
        
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