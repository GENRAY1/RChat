using Dapper;
using RChat.Domain.Accounts;
using RChat.Domain.Accounts.Repository;
using RChat.Domain.Users;
using RChat.Infrastructure.DataAccess.Connections;
using RChat.Infrastructure.DataAccess.QueryBuilders;

namespace RChat.Infrastructure.DataAccess.Repositories;

public class AccountRepository(IDbConnectionFactory connectionFactory) 
    : IAccountRepository
{
    public async Task<Account?> GetAsync(GetAccountParameters parameters)
    {
        const string defaultSql =
            $"""
             SELECT 
                 acc.id AS {nameof(Account.Id)},
                 acc.login AS {nameof(Account.Login)},
                 acc.password AS {nameof(Account.Password)},
                 acc.role_id AS {nameof(Account.RoleId)},
                 acc.created_at AS {nameof(Account.CreatedAt)},
                 role.id AS {nameof(AccountRole.Id)},
                 role.name AS {nameof(AccountRole.Name)},
                 role.description AS {nameof(AccountRole.Description)},
                 u.id AS {nameof(Account.User.Id)},
                 u.username AS {nameof(Account.User.Username)},
                 u.description AS {nameof(Account.User.Description)},
                 u.date_of_birth AS {nameof(Account.User.DateOfBirth)},
                 u.created_at AS {nameof(Account.User.CreatedAt)},
                 u.updated_at AS {nameof(Account.User.UpdatedAt)},
                 u.account_id AS {nameof(Account.User.AccountId)},
                 u.firstname AS {nameof(Account.User.Firstname)},
                 u.lastname AS {nameof(Account.User.Lastname)}
             FROM public.account AS acc
             JOIN public.account_role AS role ON role.id = acc.role_id
             LEFT JOIN public.user AS u ON u.account_id = acc.id
             """;

        using var connection = await connectionFactory.CreateAsync();
        
        QueryBuilder queryBuilder = new QueryBuilder(defaultSql);

        if (parameters.Id.HasValue)
        {
            queryBuilder.AddCondition("acc.id = @Id");
            queryBuilder.AddParameter("@Id", parameters.Id.Value);
        }
        
        if (parameters.Login is not null)
        {
            queryBuilder.AddCondition("acc.login = @Login");
            queryBuilder.AddParameter("@Login", parameters.Login);
        }
        
        var accountQuery = await connection.QueryAsync<Account, AccountRole, User?, Account>(
            queryBuilder.BuildQuery(),
            (account, role, user) =>
            {
                account.AccountRole = role;
                account.User = user;
                return account;
            },
            param:queryBuilder.GetParameters(),
            splitOn: $"{nameof(Account.Id)}, {nameof(AccountRole.Id)}, {nameof(User.Id)}");


        return accountQuery.FirstOrDefault();
    }

    public async Task<int> CreateAsync(Account account)
    {
        const string sql =
            $"""
             INSERT INTO public.account
             (login, password, role_id, created_at)
             VALUES (
                @{nameof(Account.Login)},
                @{nameof(Account.Password)},
                @{nameof(Account.RoleId)},
                @{nameof(Account.CreatedAt)}
             )
             RETURNING id;
             """;

        using var connection = await connectionFactory.CreateAsync();

        return await connection.ExecuteScalarAsync<int>(sql, account);
    }
}