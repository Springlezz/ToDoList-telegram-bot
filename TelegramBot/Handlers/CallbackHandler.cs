using Telegram.Bot.Types;
using TelegramBot.Scenario;
using Telegram.Bot.Types.Enums;
using TelegramBot.Handlers;

namespace TelegramBot.Handlers;

public class CallbackHandler : ICallbackHandler
{
    private readonly ICallbackScenario _scenario;

    public CallbackHandler(ICallbackScenario scenario)
    {
        _scenario = scenario;
    }

    public Task HandleAsync(CallbackQuery callback)
    {
        return _scenario.HandleAsync(callback);
    }
}