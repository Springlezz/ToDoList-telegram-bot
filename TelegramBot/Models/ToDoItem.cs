namespace TelegramBot.Models;

public class ToDoItem
{
    public Guid Id { get; set; }
    
    public long ChatId { get; set; }

    public string ToDoItemText { get; set; }
    public bool IsDone { get; set; }
    
    public long CreatedByUserId { get; set; }
    public DateTime? CompletedAt { get; set; }

    public DateOnly Day { get; set; }
}