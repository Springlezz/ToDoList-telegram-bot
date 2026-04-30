using TelegramBot.Repository.Interface;
using TelegramBot.Models;
using TelegramBot.Data.Entities;
using TelegramBot.Data;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Repository;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _dbContext;
    
    public UserRepository (AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<UserEntity>> GetUsersWithBirthdayTodayAsync()
    {
        var today = DateTime.Today;
        
        return await  _dbContext.Users
            .Where(u => u.HappyBirthday.HasValue && u.HappyBirthday.Value.Day == today.Day && u.HappyBirthday.Value.Month == today.Month)
            .ToListAsync();
    }

    public async Task<UserEntity?> GetByTelegramIdAsync(long telegramUserId)
    {
       return await _dbContext.Users
            .FirstOrDefaultAsync(u => u.TelegramUserId == telegramUserId);
    }

    public async Task AddAsync(UserEntity user)
    {
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(UserEntity user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }
    
}