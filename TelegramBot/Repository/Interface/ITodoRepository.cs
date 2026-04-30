using Telegram.Bot.Types;
using TelegramBot.Data.Entities;
using TelegramBot.Models;

namespace TelegramBot.Repository.Interface;

public interface ITodoRepository
{
    Task<ToDoItemEntity?> GetByIdAsync(Guid id);
    Task<IEnumerable<ToDoItemEntity>> GetAllAsync(long chatId);
    Task<IEnumerable<ToDoItemEntity>> GetByDayAsync(long chatId, DateOnly day);

    Task AddAsync(ToDoItemEntity todoItem);
    Task UpdateAsync(ToDoItemEntity todoItem);
    Task DeleteAsync(ToDoItemEntity todoItem);
}