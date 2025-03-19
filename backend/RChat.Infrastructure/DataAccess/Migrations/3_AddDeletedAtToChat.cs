using FluentMigrator;

namespace RChat.Infrastructure.DataAccess.Migrations;

[Migration(3)]
public class AddDeletedAtToChatGroup : Migration
{
    public override void Up()
    {
        Alter.Table("chat")
            .AddColumn("deleted_at").AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Column("deleted_at").FromTable("chat");
    }
}