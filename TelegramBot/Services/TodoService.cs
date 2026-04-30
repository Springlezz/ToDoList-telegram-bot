using TelegramBot.Data.Entities;
using TelegramBot.Repository.Interface;
using TelegramBot.Models;
using TelegramBot.Services.Interface;
using TelegramBot.Data;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Services;

public class TodoService : ITodoService
{
    private readonly ITodoRepository _repository;

    private readonly AppDbContext _dbContext;
    public TodoService(AppDbContext dbContext, ITodoRepository repository)
    {
        _repository = repository;
        _dbContext = dbContext;
    }

    public async Task<ToDoItem?> GetByIdAsync(long chatId, Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null || entity.ChatId != chatId)
        {
            return null;
        }

        return new ToDoItem
        {
            Id = entity.Id,
            ChatId = entity.ChatId,
            CreatedByUserId =  entity.CreatedByUserId,
            ToDoItemText = entity.ToDoItemText,
            IsDone = entity.IsDone,
            CompletedAt = entity.CompletedAt,
            Day = entity.Day
        };
    }

    public async Task<IEnumerable<ToDoItem>> GetAllAsync(long chatId)
    {
        var entities = await _repository.GetAllAsync(chatId);

        return entities.Select(e => new ToDoItem
        {
            Id = e.Id,
            ChatId = e.ChatId,
            CreatedByUserId =  e.CreatedByUserId,
            ToDoItemText = e.ToDoItemText,
            IsDone = e.IsDone,
            CompletedAt = e.CompletedAt,
            Day = e.Day
        });
    }

    public async Task<IEnumerable<ToDoItem>> GetByDayAsync(long chatId, DateOnly day)
    {
        var entities = await _repository.GetByDayAsync(chatId, day);

        return entities.Select(e => new ToDoItem
        {
            Id = e.Id,
            ChatId = e.ChatId,
            CreatedByUserId = e.CreatedByUserId,
            ToDoItemText = e.ToDoItemText,
            IsDone = e.IsDone,
            CompletedAt = e.CompletedAt,
            Day = e.Day
        });
    }
    
    public async Task UpdateAsync(long chatId, Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null || entity.ChatId != chatId)
        {
            return;
        }
        
        entity.IsDone = true;
        entity.CompletedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(entity);
    }

    public async Task DeleteAsync(long chatId, Guid id)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (entity == null || entity.ChatId != chatId)
        {
            return;
        }
        
        await _repository.DeleteAsync(entity);
    }

    public async Task AddAsync(long chatId, long userId, string text)
    {
        var entity = new ToDoItemEntity
        {
            ChatId = chatId,
            CreatedByUserId = userId,
            ToDoItemText = text,
            IsDone = false,
            Day = DateOnly.FromDateTime(DateTime.UtcNow)
        };

        await _repository.AddAsync(entity);
    }
    
    public async Task ToggleAsync(int todoId)
    {
        var item = await _dbContext.ToDoItems.FindAsync(todoId);
        if (item == null) return;

        item.IsDone = !item.IsDone;

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task ClearByDayAsync(long chatId, DateOnly day)
    {
        var items = await _dbContext.ToDoItems
            .Where(x => x.ChatId == chatId && x.Day == day)
            .ToListAsync();

        _dbContext.ToDoItems.RemoveRange(items);

        await _dbContext.SaveChangesAsync();
    }
}