using Telegram.Bot.Types;
using TelegramBot.Handlers;

namespace TelegramBot.Handlers;
public class UpdateHandler : IUpdateHandler
{
    private readonly IMessageHandler _messageHandler;
    private readonly ICallbackHandler _callbackHandler;

    public UpdateHandler(
        IMessageHandler messageHandler,
        ICallbackHandler callbackHandler)
    {
        _messageHandler = messageHandler;
        _callbackHandler = callbackHandler;
    }

    public async Task HandleAsync(Update update)
    {
        if (update.Message is not null)
        {
            await _messageHandler.HandleAsync(update.Message);
        }

        if (update.CallbackQuery != null)
        {
            await _callbackHandler.HandleAsync(update.CallbackQuery);
        }
    }
}