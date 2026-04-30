using Telegram.Bot.Types;

namespace TelegramBot.Handlers;

public interface IUpdateHandler
{
    Task HandleAsync(Update update);
}