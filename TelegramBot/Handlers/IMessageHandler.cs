using Telegram.Bot.Types;

namespace TelegramBot.Handlers;

public interface IMessageHandler
{
    Task HandleAsync(Message message);
}