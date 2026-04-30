namespace TelegramBot.Data.Entities;

public class MessageEntity : BaseEntity
{
    public DateTime? MessageDate { get; set; }
    
    public long ChatId { get; set; }
    
    public long? TelegramUserId { get; set; }
    public string? Username { get; set; }
    
    public string MessageText { get; set; }
}