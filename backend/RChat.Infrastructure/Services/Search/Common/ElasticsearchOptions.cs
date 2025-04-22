namespace RChat.Infrastructure.Services.Search.Common;

public class ElasticsearchOptions
{
    public required string Url { get; set; }
    
    public required string Username { get; set; }
    
    public required string Password { get; set; }
}