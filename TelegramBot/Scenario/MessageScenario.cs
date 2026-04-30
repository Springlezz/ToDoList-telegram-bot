using Telegram.Bot.Types;
using TelegramBot.Scenario;
using TelegramBot.Services.Interface;
using TelegramBot.Handlers;
using TelegramBot.Models;
using TelegramBot.Data.Entities;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Scenario;

public class MessageScenario : IMessageScenario
{
    private readonly IUserService _userService;
    private readonly ITodoService _todoService;
    private readonly ITelegramSender _telegramSender;
    private readonly ITodoMessageBuilder _todoMessageBuilder;

    public MessageScenario(
        IUserService userService,
        ITodoService todoService,
        ITelegramSender telegramSender,
        ITodoMessageBuilder todoMessageBuilder)
    {
        _userService = userService;
        _todoService = todoService;
        _telegramSender = telegramSender;
        _todoMessageBuilder = todoMessageBuilder;
    }

    public async Task HandleAsync(Message message)
    {
        if (message?.Text == null)
        {
            return;
        }
           

        if (string.IsNullOrWhiteSpace(message.Text))
        {
            return;
        }
            

        if (message.From.IsBot)
        {
            return;
        }
            

        var chatId = message.Chat.Id;
        var userId = message.From.Id;

        var user = await _userService.GetOrCreateAsync(userId, message.From.Username);
        
        if (user.State == UserState.WaitingBirthday)
        {
            if (DateOnly.TryParse(message.Text, out var birthday))
            {
                user.HappyBirthday = birthday;
                user.State = UserState.None;

                await _userService.UpdateAsync(user);

                await _telegramSender.SendTextAsync(chatId,
                    "🎉 Дата рождения сохранена");

                return;
            }

            await _telegramSender.SendTextAsync(chatId,
                "❌ Неверный формат. Пример: 2001.05.17");

            return;
        }
        
        if (message.Text.StartsWith("/start"))
        {
            await _telegramSender.SendTextAsync(chatId,
                "Привет! Я бот для списка дел.\nОзнакомиться с командами можно при помощи /help");

            if (user.HappyBirthday == null)
            {
                user.State = UserState.WaitingBirthday;
                await _userService.UpdateAsync(user);

                await _telegramSender.SendTextAsync(
                    chatId,
                    "📅 Введите дату рождения",
                    _todoMessageBuilder.BuildBirthdayQuestionKeyboard());
            }

            return;
        }

        if (message.Text.StartsWith("/todo"))
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var items = await _todoService.GetByDayAsync(chatId, today);

            var text = _todoMessageBuilder.BuildDayText(items, today);
            var keyboard = _todoMessageBuilder.BuildDayKeyboard(today);

            await _telegramSender.SendTextAsync(chatId, text, keyboard);

            return;
        }

        if (message.Text.StartsWith("/list"))
        {
            var items = await _todoService.GetAllAsync(chatId);

            if (!items.Any())
            {
                await _telegramSender.SendTextAsync(chatId,"Список пуст 📭");
                return;
            }

            var text = "Вот все ваши задачи:\n";
            foreach (var group in items
                         .OrderByDescending(i => i.Day)
                         .GroupBy(i => i.Day))
            {
                text += $"\n📅 {group.Key:dd MMMM, dddd, yyyy}\n";
                foreach (var item in group)
                {
                    text += $"▪️ {item.ToDoItemText}\n";
                }
            }

            await _telegramSender.SendTextAsync(chatId, text);
            return;
        }
        

        if (message.Text.StartsWith("/addtodo"))
        {
            var text = message.Text.Replace("/addtodo", "").Trim();

            if (string.IsNullOrWhiteSpace(text))
            {
                await _telegramSender.SendTextAsync(chatId,
                    "Чтобы добавить дело в список вам нужно написать: /addtodo {название дела}");
                return;
            }

            var day = user.SelectedTodoDay 
                      ?? DateOnly.FromDateTime(DateTime.UtcNow);

            await _todoService.AddAsync(chatId, userId, text, day);

            await _telegramSender.SendTextAsync(chatId, "✅ Добавлено");

            return;
        }
    }
}