using System.Data;
using FluentMigrator;
using FluentMigrator.Postgres;
using RChat.Domain.Accounts;
using RChat.Domain.Users;

namespace RChat.Infrastructure.Data.Migrations;

[Migration(1)]
public class AddUser : Migration{
    public override void Up()
    {
        Create.Sequence("user_id_seq")
            .IncrementBy(1)
            .StartWith(100000);
        
        Create.Table("user")
            .WithColumn("id").AsInt32().PrimaryKey().NotNullable()
            .WithColumn("username").AsString(User.MaxUsernameLength).NotNullable()
            .WithColumn("login").AsString().NotNullable()
            .WithColumn("password").AsString().NotNullable()
            .WithColumn("description").AsString(User.MaxDescriptionLength).Nullable()
            .WithColumn("date_of_birth").AsDate().Nullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().Nullable()
            .WithColumn("role_id").AsInt32().NotNullable();
        
        // Установка значения для id из последовательности
        Execute.Sql("ALTER TABLE \"user\" ALTER COLUMN id SET DEFAULT nextval('user_id_seq');");
        
        // Триггер для обновления username, если он не указан
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
        
        Create.Table("user_role")
            .WithColumn("id").AsInt32().PrimaryKey()
            .WithColumn("name").AsString(AccountRole.MaxNameLength).NotNullable()
            .WithColumn("description").AsString(AccountRole.MaxDescriptionLength).Nullable();
        
        Insert.IntoTable("user_role").Row(new { id = AccountRole.User.Id, name = AccountRole.User.Name, description = AccountRole.User.Description });
        Insert.IntoTable("user_role").Row(new { id = AccountRole.Admin.Id, name = AccountRole.Admin.Name, description = AccountRole.Admin.Description });

        Create.ForeignKey()
            .FromTable("user").ForeignColumn("role_id")
            .ToTable("user_role").PrimaryColumn("id");
    }

    public override void Down()
    {
        // Удалить триггер и функцию
        Execute.Sql(@"
            DROP TRIGGER IF EXISTS set_username_trigger ON ""user"";
            DROP FUNCTION IF EXISTS set_default_username();
        ");
        
        // Удалить последовательность
        Execute.Sql("DROP SEQUENCE IF EXISTS user_id_seq;");
        
        Delete.Table("user_role");
        Delete.Table("user");
    }
}