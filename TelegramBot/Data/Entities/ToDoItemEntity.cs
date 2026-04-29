namespace TelegramBot.Data.Entities;

public class ToDoItem : BaseEntity
{
    public Guid UserEntityId { get; set; }
    public UserEntity User { get; set; }
    
    public string ToDoItemText { get; set; }
    public bool IsDone { get; set; }
    
    // public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; } 
    
    public DateOnly Day { get; set; }
    
    // public DateTime? DeadLine { get; set; }
}