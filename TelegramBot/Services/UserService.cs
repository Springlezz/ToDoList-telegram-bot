using TelegramBot.Repository.Interface;
using TelegramBot.Services.Interface;
using TelegramBot.Data.Entities;
using TelegramBot.Repository.Interface;
using TelegramBot.Repository;

namespace TelegramBot.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
    
    public async Task UpdateAsync(UserEntity user)
    {
        await _repository.UpdateAsync(user);
    }

    public async Task<UserEntity> GetOrCreateAsync(long telegramUserId, string? telegramUsername)
    {
        var user = await _repository.GetByTelegramIdAsync(telegramUserId);

        if (user != null)
        {
            if (user.TelegramUsername != telegramUsername)
            {
                user.TelegramUsername = telegramUsername;
                await _repository.UpdateAsync(user);
            }

            return user;
        }

        user = new UserEntity
        {
            TelegramUserId = telegramUserId,
            TelegramUsername = telegramUsername
        };

        await _repository.AddAsync(user);
        return user;
    }
    
    public async Task<List<UserEntity>> GetBirthdayUsersTodayAsync()
    {
        return await _repository.GetUsersWithBirthdayTodayAsync();
    }

    public async Task SetBirthdayAsync(long telegramUserId, DateOnly date)
    {
        var user = await _repository.GetByTelegramIdAsync(telegramUserId);
        if (user == null)
        {
            return;
        }

        user.HappyBirthday = date;
        await _repository.UpdateAsync(user);;
    }
}