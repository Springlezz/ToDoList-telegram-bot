using Telegram.Bot.Types.Enums;
using TelegramBot.Data.Entities;
using Telegram.Bot.Types;
using TelegramBot.Handlers;
using TelegramBot.Models;
using TelegramBot.Services;
using TelegramBot.Services.Interface;

namespace TelegramBot.BackgroundServices;

public class BirthdayReminderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public BirthdayReminderService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            using var scope = _scopeFactory.CreateScope();

            var userService = scope.ServiceProvider.GetRequiredService<IUserService>();
            var todoService = scope.ServiceProvider.GetRequiredService<ITodoService>();
            var telegramSender = scope.ServiceProvider.GetRequiredService<ITelegramSender>();
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var users = await userService.GetBirthdayUsersTodayAsync();

            foreach (var user in users)
            {
                if (user.LastBirthdayNotified?.Date == DateTime.UtcNow.Date)
                {
                    continue;
                }
                    

                await telegramSender.SendTextAsync(
                    user.TelegramUserId,
                    $"С днем рождения, {user.TelegramUsername}! 🎉\nЯ добавил напоминание купить тортик!");

                user.LastBirthdayNotified = DateTime.UtcNow;
                await userService.UpdateAsync(user);

                await todoService.AddAsync(
                    user.TelegramUserId,
                    user.TelegramUserId,
                    "Купить себе торт", today);
            }

            await Task.Delay(TimeSpan.FromHours(12), stoppingToken);
        }
    }
}