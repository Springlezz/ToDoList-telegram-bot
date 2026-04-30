using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers;

namespace TelegramBot.Scenario;

public interface ICallbackScenario
{
    Task HandleAsync(CallbackQuery callback);
}