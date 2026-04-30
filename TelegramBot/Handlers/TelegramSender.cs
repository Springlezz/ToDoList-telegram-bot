using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Handlers;

public class TelegramSender : ITelegramSender
{
    private readonly ITelegramBotClient _botClient;

    public TelegramSender(ITelegramBotClient botClient)
    {
        _botClient = botClient;
    }

    public async Task SendTextAsync(long chatId, string text, InlineKeyboardMarkup? keyboard = null)
    {
        await _botClient.SendMessage(
            chatId: chatId,
            text: text,
            replyMarkup: keyboard);
    }
    
    public async Task EditMessageAsync(
        long chatId,
        int messageId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        await _botClient.EditMessageText(
            chatId: chatId,
            messageId: messageId,
            text: text,
            replyMarkup: replyMarkup
        );
    }
    
    public async Task AnswerCallbackAsync(string callbackId)
    {
        await _botClient.AnswerCallbackQuery(
            callbackQueryId: callbackId);
    }
}