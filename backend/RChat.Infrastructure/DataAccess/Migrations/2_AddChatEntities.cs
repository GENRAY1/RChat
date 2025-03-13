using FluentMigrator;
using RChat.Domain.Chats;
using RChat.Domain.Messages;

namespace RChat.Infrastructure.Data.Migrations;

[Migration(2)]
public class AddChatEntities : Migration{
    public override void Up()
    {
        Create.Table("chat")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("type").AsInt32().NotNullable()
            .WithColumn("creator_id").AsInt32().NotNullable()
            .WithColumn("created_at").AsDateTime().NotNullable();
        
        Create.Table("chat_group")
            .WithColumn("chat_id").AsInt32().NotNullable().PrimaryKey()
            .WithColumn("name").AsString(ChatGroup.MaxNameLength).NotNullable()
            .WithColumn("description").AsString(ChatGroup.MaxDescriptionLength).Nullable()
            .WithColumn("is_private").AsBoolean().NotNullable();
        
        Create.ForeignKey()
            .FromTable("chat_group").ForeignColumn("chat_id")
            .ToTable("chat").PrimaryColumn("id");
            
        Create.Table("member")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("user_id").AsInt32().NotNullable()
            .WithColumn("chat_id").AsInt32().NotNullable()
            .WithColumn("joined_at").AsDateTime().NotNullable();
        
        Create.ForeignKey()
            .FromTable("member").ForeignColumn("chat_id")
            .ToTable("chat").PrimaryColumn("id");
        
        Create.ForeignKey()
            .FromTable("member").ForeignColumn("user_id")
            .ToTable("user").PrimaryColumn("id");
        
        Create.Table("message")
            .WithColumn("id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("text").AsString(Message.MaxTextLength).NotNullable()
            .WithColumn("chat_id").AsInt32().NotNullable()
            .WithColumn("sender_id").AsInt32().NotNullable()
            .WithColumn("reply_to_message_id").AsInt32().Nullable()
            .WithColumn("created_at").AsDateTime().NotNullable()
            .WithColumn("updated_at").AsDateTime().Nullable();
        
        Create.ForeignKey()
            .FromTable("message").ForeignColumn("chat_id")
            .ToTable("chat").PrimaryColumn("id");
        
        Create.ForeignKey()
            .FromTable("message").ForeignColumn("sender_id")
            .ToTable("user").PrimaryColumn("id");
        
        Create.ForeignKey()
            .FromTable("message").ForeignColumn("reply_to_message_id")
            .ToTable("message").PrimaryColumn("id");
    }

    public override void Down()
    {
        Delete.Table("chat");
        Delete.Table("chat_group");
        Delete.Table("member");
        Delete.Table("message");
    }
}