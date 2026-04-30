using TelegramBot.Data.Entities;

namespace TelegramBot.Services.Interface;

public interface IUserService
{
    Task<UserEntity> GetOrCreateAsync(long telegramUserId, string? telegramUsername);
    Task SetBirthdayAsync(long telegramUserId, DateOnly date);
    Task<List<UserEntity>> GetBirthdayUsersTodayAsync();
    Task UpdateAsync(UserEntity user);
}