using Telegram.Bot.Types.Enums;
using TelegramBot.Services.Interface;
using TelegramBot.Services;
using TelegramBot.Scenario;
using Telegram.Bot.Types;

namespace TelegramBot.Handlers;

public class MessageHandler : IMessageHandler
{
    private readonly IUserService _userService;
    private readonly IMessageScenario _messageScenario;

    public MessageHandler(
        IUserService userService,
        IMessageScenario messageScenario)
    {
        _userService = userService;
        _messageScenario = messageScenario;
    }
    

    public async Task HandleAsync(Message message)
    {
        if (message.From is null)
        {
            return;
        }
            

        await _userService.GetOrCreateAsync(
            message.From.Id,
            message.From.Username);

        await _messageScenario.HandleAsync(message);
    }
}