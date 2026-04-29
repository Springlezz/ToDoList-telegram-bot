using TelegramBot.Data;
using TelegramBot.Data.Entities;
using TelegramBot.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;
using TelegramBot.Repository.Interface;

namespace TelegramBot.Repository;

public class Repository : IMessageRepository
{
    private readonly AppDbContext _dbContext;
    
    public Repository(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<MessageEntity?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Messages
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task AddAsync(MessageEntity message)
    {
        _dbContext.Messages.Add(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(MessageEntity message)
    {
        _dbContext.Messages.Update(message);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(MessageEntity message)
    {
        _dbContext.Messages.Remove(message);
        await _dbContext.SaveChangesAsync();
    }
}