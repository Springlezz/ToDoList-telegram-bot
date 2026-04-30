using TelegramBot.Models;
using TelegramBot.Data.Entities;
namespace TelegramBot.Repository.Interface;

public interface IUserRepository
{
    Task<UserEntity?> GetByTelegramIdAsync(long telegramUserId);
    Task AddAsync(UserEntity user);
    Task UpdateAsync(UserEntity user);
    Task<List<UserEntity>> GetUsersWithBirthdayTodayAsync();
    
}