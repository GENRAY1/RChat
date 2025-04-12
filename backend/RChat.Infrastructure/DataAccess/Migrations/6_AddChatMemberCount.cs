using FluentMigrator;

namespace RChat.Infrastructure.DataAccess.Migrations;

[Migration(6)]
public class AddChatMemberCount : Migration{
    public override void Up()
    {
        Create.Column("member_count")
            .OnTable("chat")
            .AsInt32()
            .WithDefaultValue(0)
            .NotNullable();
        
        // Создание функции
        Execute.Sql(@"
        CREATE OR REPLACE FUNCTION update_chat_member_count()
        RETURNS TRIGGER AS $$
        BEGIN
            IF TG_OP = 'INSERT' THEN
                UPDATE chat
                SET member_count = member_count + 1
                WHERE id = NEW.chat_id;
            ELSIF TG_OP = 'DELETE' THEN
                UPDATE chat
                SET member_count = GREATEST(member_count - 1, 0)
                WHERE id = OLD.chat_id;
            END IF;
            RETURN NULL;
        END;
        $$ LANGUAGE plpgsql;
    ");

        // Создание триггера на INSERT
        Execute.Sql(@"
        CREATE TRIGGER trg_member_insert
        AFTER INSERT ON member
        FOR EACH ROW
        EXECUTE FUNCTION update_chat_member_count();
    ");

        // Создание триггера на DELETE
        Execute.Sql(@"
        CREATE TRIGGER trg_member_delete
        AFTER DELETE ON member
        FOR EACH ROW
        EXECUTE FUNCTION update_chat_member_count();
    ");
    }

    public override void Down()
    {
        Delete.Column("member_count").FromTable("chat");
        Execute.Sql("DROP TRIGGER IF EXISTS trg_member_insert ON member;");
        Execute.Sql("DROP TRIGGER IF EXISTS trg_member_delete ON member;");
        Execute.Sql("DROP FUNCTION IF EXISTS update_chat_member_count();");
    }
}