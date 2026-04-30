namespace TelegramBot.Models;

public class User
{
    public string? AliceUsername { get; set; }
    public string? TelegramUsername { get; set; }
    
    public long? AliceUserId { get; set; } 
    public long TelegramUserId { get; set; }
    public DateOnly? SelectedTodoDay { get; set; }
    
    public DateOnly? HappyBirthday { get; set; }
}