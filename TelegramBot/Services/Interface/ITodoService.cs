using TelegramBot.Data.Entities;
using TelegramBot.Models;
namespace TelegramBot.Services.Interface;

public interface ITodoService
{
    Task<ToDoItem?> GetByIdAsync(long chatId, Guid id);
    Task<IEnumerable<ToDoItem>> GetAllAsync(long chatId);
    Task<IEnumerable<ToDoItem>> GetByDayAsync(long chatId, DateOnly day);

    Task AddAsync(long chatId, long userId, string todotemText);
    Task UpdateAsync(long chatId, Guid id);
    Task DeleteAsync(long chatId, Guid id);
    Task ClearByDayAsync(long chatId, DateOnly day);
    Task ToggleAsync(int todoId);
}