using RChat.Application.Chats.Dtos;
using RChat.Domain.Chats;

namespace RChat.Web.Controllers.Chats.Create;

public class CreateChatRequest
{
    public required ChatType Type { get; set; }
    
    public ChatGroupDto? GroupChat { get; set; }
    
    public int? RecipientId { get; init; } 
}