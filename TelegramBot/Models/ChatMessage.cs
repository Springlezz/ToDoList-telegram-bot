namespace TelegramBot.Models;

public class ChatMessage
{
    public DateTime? MessageDate { get; set; }
    
    public long ChatId { get; set; }
    
    public long? TelegramUserId { get; set; }
    public string? Username { get; set; }
    
    public string MessageText { get; set; }
}