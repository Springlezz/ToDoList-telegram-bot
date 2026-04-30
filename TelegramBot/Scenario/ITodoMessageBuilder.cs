using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;

namespace TelegramBot.Scenario;

public interface ITodoMessageBuilder
{
    string BuildDayText(IEnumerable<ToDoItem> items, DateOnly day);

    InlineKeyboardMarkup BuildDayKeyboard(DateOnly day);

    InlineKeyboardMarkup BuildBirthdayQuestionKeyboard();
    
}