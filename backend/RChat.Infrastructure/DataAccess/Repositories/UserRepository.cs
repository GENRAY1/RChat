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
        StringBuilder sql = new ();
        
        const string defaultSql =
            $"""
            SELECT 
                u.id AS {nameof(User.Id)},
                u.login AS {nameof(User.Login)},
                u.username AS {nameof(User.Username)},
                u.password AS {nameof(User.Password)},
                u.description AS {nameof(User.Description)},
                u.date_of_birth AS {nameof(User.DateOfBirth)},
                u.created_at AS {nameof(User.CreatedAt)},
                u.updated_at AS {nameof(User.UpdatedAt)},
                u.role_id AS {nameof(User.RoleId)},
                r.id AS {nameof(User.UserRole.Id)},
                r.name AS {nameof(User.UserRole.Name)},
                r.description AS {nameof(User.UserRole.Description)}
            FROM public.user AS u
            JOIN public.user_role AS r ON u.role_id = r.id
            WHERE TRUE
            """;
        
        sql.Append(defaultSql);

        var param = new DynamicParameters();

        if (parameters.Id is not null)
        {
            sql.Append($" AND u.id = @Id");
            param.Add("@Id", parameters.Id);
        }

        if (parameters.Username is not null)
        {
            sql.Append($" AND u.username = @Username");
            param.Add("@Username", parameters.Username);
        }

        if (parameters.Login is not null)
        {
            sql.Append($" AND u.login = @Login");
            param.Add("@Login", parameters.Login);
        }
        
        using var connection = await connectionFactory.CreateAsync();
        
        User? user = (await connection.QueryAsync<User, UserRole, User>(
            sql.ToString(),
            (user, role) =>
            {
                user.UserRole = role;
                
                return user;
            },
            param,
            splitOn: $"{nameof(User.Id)}, {nameof(User.UserRole.Id)}"
        )).FirstOrDefault();
        
        return user;
    }

    public async Task<List<User>> GetListAsync(GetUserListParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 u.id AS {nameof(User.Id)},
                 u.login AS {nameof(User.Login)},
                 u.username AS {nameof(User.Username)},
                 u.password AS {nameof(User.Password)},
                 u.description AS {nameof(User.Description)},
                 u.date_of_birth AS {nameof(User.DateOfBirth)},
                 u.created_at AS {nameof(User.CreatedAt)},
                 u.updated_at AS {nameof(User.UpdatedAt)},
                 u.role_id AS {nameof(User.RoleId)},
                 r.id AS {nameof(User.UserRole.Id)},
                 r.name AS {nameof(User.UserRole.Name)},
                 r.description AS {nameof(User.UserRole.Description)}
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
                { UserSortingColumn.Username, "u.username" },
                { UserSortingColumn.RoleName, "r.name" },
            };
            
            queryBuilder.AddSorting(sortingColumnDbMapping, parameters.Sorting);
        }
        
        queryBuilder.AddJoin("JOIN public.user_role AS r ON u.role_id = r.id");
        
        using var connection = await connectionFactory.CreateAsync();
        
        IEnumerable<User> users = await connection.QueryAsync<User, UserRole, User>(
            queryBuilder.BuildQuery(),
            (user, role) =>
            {
                user.UserRole = role;
                return user;
            }, 
            param: queryBuilder.GetParameters(),
            splitOn: $"{nameof(User.Id)}, {nameof(User.UserRole.Id)}"
        );
        
        return users.ToList();
    }

    public async Task<int> CreateAsync(User user)
    {
        const string sql =
            $"""
            INSERT INTO public.user
            (login, username, password, description, date_of_birth, created_at, role_id)
            VALUES (
               @{nameof(User.Login)},
               @{nameof(User.Username)}, 
               @{nameof(User.Password)},
               @{nameof(User.Description)},
               @{nameof(User.DateOfBirth)},
               @{nameof(User.CreatedAt)},
               @{nameof(User.RoleId)}
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
             SET login = @{nameof(User.Login)},
             username = @{nameof(User.Username)},
             password = @{nameof(User.Password)},
             description = @{nameof(User.Description)},
             date_of_birth = @{nameof(User.DateOfBirth)},
             created_at = @{nameof(User.CreatedAt)},
             updated_at = @{nameof(User.UpdatedAt)},
             role_id = @{nameof(User.RoleId)}
             WHERE id = @{nameof(User.Id)};
             """;
        
        using var connection = await connectionFactory.CreateAsync();
        
        return await connection.ExecuteAsync(sql, user);;
    }
}