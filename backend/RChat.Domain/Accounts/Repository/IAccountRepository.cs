namespace RChat.Domain.Accounts.Repository;

public interface IAccountRepository
{
    public Task<Account?> GetAsync(GetAccountParameters parameters);
    
    public Task<int> CreateAsync(Account account);

    /*
     *
     *  public async Task<int> CreateAsync(User user)
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
     */

}