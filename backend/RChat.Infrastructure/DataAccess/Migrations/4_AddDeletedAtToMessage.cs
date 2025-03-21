using FluentMigrator;

namespace RChat.Infrastructure.DataAccess.Migrations;

[Migration(4)]
public class AddDeletedAtToMessage : Migration{
    public override void Up()
    {
        Alter.Table("message")
            .AddColumn("deleted_at").AsDateTime().Nullable();
    }

    public override void Down()
    {
        Delete.Column("deleted_at").FromTable("message");
    }
}