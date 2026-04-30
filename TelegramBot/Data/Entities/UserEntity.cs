namespace TelegramBot.Data.Entities;

public enum UserState
{
    None,
    WaitingBirthday,
    WaitingTodoText,
    WaitingTodoDate
}
public class UserEntity : BaseEntity
{
    public UserState State { get; set; }
    
    public string? AliceUsername { get; set; }
    public string? TelegramUsername { get; set; }
    
    public long? AliceUserId { get; set; } 
    public long TelegramUserId { get; set; }
    
    public DateOnly? HappyBirthday { get; set; }
    public DateTime? LastBirthdayNotified { get; set; }
    
    public ICollection<ToDoItemEntity> ToDoItems { get; set; } =  new List<ToDoItemEntity>();
}