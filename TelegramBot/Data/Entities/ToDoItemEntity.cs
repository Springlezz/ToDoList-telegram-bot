namespace TelegramBot.Data.Entities;

public class ToDoItemEntity : BaseEntity
{
    public long ChatId { get; set; }
    public long CreatedByUserId { get; set; }

    public string ToDoItemText { get; set; } = default!;
    public bool IsDone { get; set; }

    public DateTime? CompletedAt { get; set; }
    public DateOnly Day { get; set; }

    public UserEntity? User { get; set; }
}