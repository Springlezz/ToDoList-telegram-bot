using System.Data;
using TelegramBot.Repository.Interface;
using TelegramBot.Data;
using TelegramBot.Data.Entities;
using TelegramBot.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using Telegram.Bot.Types;

namespace TelegramBot.Repository;

public class TodoRepository : ITodoRepository
{
    private readonly AppDbContext _dbContext;
    
    public TodoRepository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ToDoItemEntity?> GetByIdAsync(Guid id)
    {
        return await _dbContext.ToDoItems.FindAsync(id);
    }

    public async Task<IEnumerable<ToDoItemEntity>> GetByDayAsync(long chatId, DateOnly day)
    {
        return await _dbContext.ToDoItems
            .Where(t => t.Day == day && t.ChatId == chatId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ToDoItemEntity>> GetAllAsync(long chatId)
    {
        return await _dbContext.ToDoItems
            .Where(t => t.ChatId == chatId)
            .ToListAsync();
    }
    
    public async Task AddAsync(ToDoItemEntity todoItem)
    {
        _dbContext.ToDoItems.Add(todoItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(ToDoItemEntity todoItem)
    {
        _dbContext.ToDoItems.Update(todoItem);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(ToDoItemEntity todoItem)
    {
        var todoitem = await _dbContext.ToDoItems
            .FirstOrDefaultAsync(t => t.Id == todoItem.Id);
        if (todoitem == null)
        {
            return;
        }
        _dbContext.ToDoItems.Remove(todoitem);
        await _dbContext.SaveChangesAsync();
    }
}
