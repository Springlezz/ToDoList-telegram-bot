using TelegramBot.Scenario;
using TelegramBot.Services.Interface;
using TelegramBot.Handlers;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Models;

namespace TelegramBot.Scenario;

public class TodoMessageBuilder : ITodoMessageBuilder
{
    public string BuildDayText(IEnumerable<ToDoItem> items, DateOnly day)
    {
        var header = $"📅 {day:dd.MM.yyyy}\n\n";

        if (!items.Any())
            return header + "Никаких дел нет, отдыхаем!";

        var body = string.Join("\n", items.Select(i =>
            i.IsDone
                ? $"▪️ {i.ToDoItemText}"
                : $"▪️ {i.ToDoItemText}" //временно
        ));

        return header + body;
    }
    
    public InlineKeyboardMarkup BuildDayKeyboard(DateOnly day)
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("⬅️", $"todo_day:{day.AddDays(-1):dd.MM.yyyy}"),

                InlineKeyboardButton.WithCallbackData("➕ Создать задачу на этот день", $"todo_add:{day:dd.MM.yyyy}"),

                InlineKeyboardButton.WithCallbackData("🗑 Очистить", $"todo_clear:{day:dd.MM.yyyy}"),

                InlineKeyboardButton.WithCallbackData("➡️", $"todo_day:{day.AddDays(1):dd.MM.yyyy}")
            }
        });
    }
    
    public InlineKeyboardMarkup BuildBirthdayQuestionKeyboard()
    {
        return new InlineKeyboardMarkup(new[]
        {
            new []
            {
                InlineKeyboardButton.WithCallbackData("🎂 Указать дату", "birthday_set")
            }
        });
    }
}