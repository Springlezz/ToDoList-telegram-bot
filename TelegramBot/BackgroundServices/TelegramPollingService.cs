using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;

namespace TelegramBot.BackgroundServices;

public class TelegramPollingService : BackgroundService
{
    private readonly ITelegramBotClient _bot;
    private readonly IUpdateHandler _handler;
    private readonly ILogger<TelegramPollingService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public TelegramPollingService(
        ITelegramBotClient bot,
        ILogger<TelegramPollingService> logger,
        IServiceScopeFactory scopeFactory)
    {
        _bot = bot;
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var offset = 0;

        while (!stoppingToken.IsCancellationRequested)
        {
            var updates = await _bot.GetUpdates(offset, timeout: 10, cancellationToken: stoppingToken);
            

            foreach (var update in updates)
            {
                offset = update.Id + 1;

                using var scope = _scopeFactory.CreateScope();

                var handler = scope.ServiceProvider.GetRequiredService<IUpdateHandler>();

                await handler.HandleAsync(update);
            }
            
        }
    }
}