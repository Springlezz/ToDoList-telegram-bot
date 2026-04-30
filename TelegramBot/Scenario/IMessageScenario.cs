using Telegram.Bot.Types;

namespace TelegramBot.Handlers;

public interface IMessageScenario
{
    Task HandleAsync(Message message);
}