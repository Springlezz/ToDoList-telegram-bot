using Telegram.Bot;
using Telegram.Bot.Exceptions;
using TelegramBot.Handlers;
using Telegram.Bot.Types.ReplyMarkups;

public class TelegramSender : ITelegramSender
{
    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<TelegramSender> _logger;

    public TelegramSender(
        ITelegramBotClient botClient,
        ILogger<TelegramSender> logger)
    {
        _botClient = botClient;
        _logger = logger;
    }

    public async Task SendTextAsync(long chatId, string text, InlineKeyboardMarkup? keyboard = null)
    {
        try
        {
            await _botClient.SendMessage(
                chatId: chatId,
                text: text,
                replyMarkup: keyboard);
        }
        catch (ApiRequestException ex)
        {
            _logger.LogWarning(ex,
                "Telegram API error while sending message to chat {ChatId}: {Message}",
                chatId, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error while sending message to chat {ChatId}",
                chatId);
        }
    }

    public async Task EditMessageAsync(
        long chatId,
        int messageId,
        string text,
        InlineKeyboardMarkup? replyMarkup = null)
    {
        try
        {
            await _botClient.EditMessageText(
                chatId: chatId,
                messageId: messageId,
                text: text,
                replyMarkup: replyMarkup);
        }
        catch (ApiRequestException ex) when (ex.Message.Contains("message is not modified"))
        {
            _logger.LogDebug(
                "Message {MessageId} in chat {ChatId} was not modified",
                messageId, chatId);
        }
        catch (ApiRequestException ex)
        {
            _logger.LogWarning(ex,
                "Telegram API error while editing message {MessageId} in chat {ChatId}: {Message}",
                messageId, chatId, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error while editing message {MessageId} in chat {ChatId}",
                messageId, chatId);
        }
    }

    public async Task AnswerCallbackAsync(string callbackId)
    {
        try
        {
            await _botClient.AnswerCallbackQuery(callbackQueryId: callbackId);
        }
        catch (ApiRequestException ex) when (ex.ErrorCode == 400)
        {
            _logger.LogDebug("Callback expired or invalid: {Message}", ex.Message);
        }
        catch (ApiRequestException ex)
        {
            _logger.LogWarning(ex,
                "Telegram API error while answering callback: {Message}",
                ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Unexpected error while answering callback");
        }
    }
}