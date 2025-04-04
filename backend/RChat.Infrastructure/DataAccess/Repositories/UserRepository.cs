using System.Text;
using Dapper;
using RChat.Domain.Users;
using RChat.Domain.Users.Repository;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.QueryBuilders;

namespace RChat.Infrastructure.DataAccess.Repositories;

public class UserRepository(IDbConnectionFactory connectionFactory) 
    : IUserRepository
{
    public async Task<User?> GetAsync(GetUserParameters parameters)
    {
        const string defaultSql =
            $"""
            SELECT 
                u.id AS {nameof(User.Id)},
                u.username AS {nameof(User.Username)},
                u.description AS {nameof(User.Description)},
                u.date_of_birth AS {nameof(User.DateOfBirth)},
                u.created_at AS {nameof(User.CreatedAt)},
                u.updated_at AS {nameof(User.UpdatedAt)},
                u.account_id AS {nameof(User.AccountId)},
                u.username AS {nameof(User.Username)},
                u.firstname AS {nameof(User.Firstname)},
                u.lastname AS {nameof(User.Lastname)}
            FROM public.user AS u
            """;
        
        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);
        
        if (parameters.Id is not null)
        {
            queryBuilder.AddCondition("u.id = @Id");
            queryBuilder.AddParameter("@Id", parameters.Id);
        }

        if (parameters.Username is not null)
        {
            queryBuilder.AddCondition("u.username = @Username");
            queryBuilder.AddParameter("@Username", parameters.Username);
        }
        
        if (parameters.AccountId is not null)
        {
            queryBuilder.AddCondition("u.account_id = @AccountId");
            queryBuilder.AddParameter("@AccountId", parameters.AccountId);
        }
        
        using var connection = await connectionFactory.CreateAsync();

        string sql = queryBuilder.BuildQuery();
        
        User? user = await connection.QueryFirstOrDefaultAsync<User>(sql, queryBuilder.GetParameters());
        
        return user;
    }

    public async Task<List<User>> GetListAsync(GetUserListParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 u.id AS {nameof(User.Id)},
                 u.username AS {nameof(User.Username)},
                 u.description AS {nameof(User.Description)},
                 u.date_of_birth AS {nameof(User.DateOfBirth)},
                 u.created_at AS {nameof(User.CreatedAt)},
                 u.updated_at AS {nameof(User.UpdatedAt)},
                 u.account_id AS {nameof(User.AccountId)},
                 u.firstname AS {nameof(User.Firstname)},
                 u.lastname AS {nameof(User.Lastname)}
             FROM public.user AS u
             """;
        
        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);

        if (parameters.Pagination is not null)
        {
            queryBuilder.AddPagination(parameters.Pagination);
        }
        
        if (parameters.Sorting is not null)
        {
            var sortingColumnDbMapping = new Dictionary<UserSortingColumn, string>
            {
                { UserSortingColumn.CreatedAt, "u.created_at" },
                { UserSortingColumn.DateOfBirth, "u.date_of_birth" },
                { UserSortingColumn.Username, "u.username" }
            };
            
            queryBuilder.AddSorting(sortingColumnDbMapping, parameters.Sorting);
        }
        
        using var connection = await connectionFactory.CreateAsync();

        IEnumerable<User> users = await connection.QueryAsync<User>(
            queryBuilder.BuildQuery(),
            param: queryBuilder.GetParameters());
        
        return users.ToList();
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql =
            $"""
            INSERT INTO public.user
            (account_id, username, firstname, lastname, description, date_of_birth, created_at)
            VALUES (
               @{nameof(User.AccountId)},
               @{nameof(User.Username)}, 
               @{nameof(User.Firstname)},
               @{nameof(User.Lastname)},
               @{nameof(User.Description)},
               @{nameof(User.DateOfBirth)},
               @{nameof(User.CreatedAt)}
            )
            RETURNING id;
            """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        return await connection.ExecuteScalarAsync<int>(sql, user);
    }

    public async Task<int> UpdateAsync(User user)
    {
        const string sql =
            $"""
             UPDATE public.user
             SET account_id = @{nameof(User.AccountId)},
             username = @{nameof(User.Username)},
             firstname = @{nameof(User.Firstname)},
             lastname = @{nameof(User.Lastname)},
             description = @{nameof(User.Description)},
             date_of_birth = @{nameof(User.DateOfBirth)},
             created_at = @{nameof(User.CreatedAt)},
             updated_at = @{nameof(User.UpdatedAt)}
             WHERE id = @{nameof(User.Id)};
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        return await connection.ExecuteAsync(sql, user);;
    }
}