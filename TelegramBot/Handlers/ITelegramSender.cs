using Telegram.Bot.Types.ReplyMarkups;

using Telegram.Bot;

namespace TelegramBot.Handlers;

public interface ITelegramSender
{
    Task SendTextAsync(long chatId, string text, InlineKeyboardMarkup? keyboard = null);

    Task EditMessageAsync(
        long chatId,
        int messageId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null);
    
    Task AnswerCallbackAsync(string callbackId);
}