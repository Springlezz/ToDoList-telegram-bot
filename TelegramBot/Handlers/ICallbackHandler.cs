using System;
using Telegram.Bot.Types;

namespace TelegramBot.Handlers;

public interface ICallbackHandler
{
    Task HandleAsync(CallbackQuery callback);
}