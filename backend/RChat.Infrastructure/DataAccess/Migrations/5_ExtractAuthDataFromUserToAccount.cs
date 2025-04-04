using FluentMigrator;
using RChat.Domain.Accounts;
using RChat.Domain.Users;

namespace RChat.Infrastructure.DataAccess.Migrations;
[Migration(5)]
public class ExtractAuthDataFromUserToAccount : Migration
{
    public override void Up()
    {
        Delete.Column("login").FromTable("user");
        Delete.Column("password").FromTable("user");
        Delete.Column("role_id").FromTable("user");
        
        Delete.Table("user_role");

        Alter.Column("username")
            .OnTable("user")
            .AsString(User.MaxUsernameLength).Nullable();

        Create.Column("firstname")
            .OnTable("user")
            .AsString(User.MaxFirstnameLength)
            .NotNullable();
        
        Create.Column("lastname")
            .OnTable("user")
            .AsString(User.MaxLastnameLength)
            .Nullable();

        Create.Column("account_id")
            .OnTable("user")
            .AsInt32()
            .Unique()
            .NotNullable();
        
        Create.Table("account")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("login").AsString().NotNullable()
            .WithColumn("password").AsString().NotNullable()
            .WithColumn("role_id").AsInt32().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable();
        
        Create.Table("account_role")
            .WithColumn("id").AsInt32().PrimaryKey()
            .WithColumn("name").AsString(AccountRole.MaxNameLength).NotNullable()
            .WithColumn("description").AsString(AccountRole.MaxDescriptionLength).Nullable();
        
        Insert.IntoTable("account_role").Row(new { id = AccountRole.User.Id, name = AccountRole.User.Name, description = AccountRole.User.Description });
        Insert.IntoTable("account_role").Row(new { id = AccountRole.Admin.Id, name = AccountRole.Admin.Name, description = AccountRole.Admin.Description });
        
        Create.ForeignKey()
            .FromTable("account").ForeignColumn("role_id")
            .ToTable("account_role").PrimaryColumn("id");
        
        Create.ForeignKey()
            .FromTable("user").ForeignColumn("account_id")
            .ToTable("account").PrimaryColumn("id");
        
        // Удалить триггер и функцию для username
        Execute.Sql(@"
            DROP TRIGGER IF EXISTS set_username_trigger ON ""user"";
            DROP FUNCTION IF EXISTS set_default_username();
        ");
    }

    public override void Down()
    {
        // Удаляем таблицу account_role
        Delete.Table("account_role");
    
        // Удаляем таблицу account
        Delete.Table("account");
    
        // Удаляем добавленные столбцы в user
        Delete.Column("firstname").FromTable("user");
        Delete.Column("lastname").FromTable("user");
    
        // Восстанавливаем оригинальную структуру user
        Alter.Column("username")
            .OnTable("user")
            .AsString(User.MaxUsernameLength).NotNullable(); // Возвращаем NOT NULL
    
        // Восстанавливаем таблицу user_role
        Create.Table("user_role")
            .WithColumn("id").AsInt32().PrimaryKey()
            .WithColumn("name").AsString(AccountRole.MaxNameLength).NotNullable()
            .WithColumn("description").AsString(AccountRole.MaxDescriptionLength).Nullable();
    
        // Восстанавливаем удалённые столбцы в user
        Create.Column("login").OnTable("user").AsString().NotNullable();
        Create.Column("password").OnTable("user").AsString().NotNullable();
        Create.Column("role_id").OnTable("user").AsInt32().NotNullable();
    
        // Восстанавливаем данные ролей (если нужно)
        Insert.IntoTable("user_role")
            .Row(new { id = AccountRole.User.Id, name = AccountRole.User.Name, description = AccountRole.User.Description });
        Insert.IntoTable("user_role")
            .Row(new { id = AccountRole.Admin.Id, name = AccountRole.Admin.Name, description = AccountRole.Admin.Description });
    
        // Восстанавливаем внешний ключ user -> user_role
        Create.ForeignKey()
            .FromTable("user").ForeignColumn("role_id")
            .ToTable("user_role").PrimaryColumn("id");
        
        // Восстанавливаем Триггер и функцию для обновления username
        Execute.Sql(@"
            CREATE OR REPLACE FUNCTION set_default_username()
            RETURNS TRIGGER AS $$
            BEGIN
                IF NEW.username IS NULL THEN
                    NEW.username := 'user_' || NEW.id;
                END IF;
                RETURN NEW;
            END;
            $$ LANGUAGE plpgsql;

            CREATE TRIGGER set_username_trigger 
            BEFORE INSERT ON ""user""
            FOR EACH ROW
            EXECUTE FUNCTION set_default_username();
        ");
    }
}