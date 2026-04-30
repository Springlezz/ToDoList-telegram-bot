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
        var chatId = message.Chat.Id;
        var userId = message.From.Id;

        var user = await _userService.GetOrCreateAsync(userId, message.From.Username);

        if (message.Text == "/start")
        {
            await _telegramSender.SendTextAsync(chatId, "Привет! Я бот, который поможет вам составить список дел.\n\n Также меня можно добавить в беседу, чтобы вы могли составить общий список дел со своими друзьями!");
            
            while (user.HappyBirthday == null)
            {
                await _telegramSender.SendTextAsync(
                    chatId,
                    "Чтобы мы могли продолжить, введи свой день рождения",
                    _todoMessageBuilder.BuildBirthdayQuestionKeyboard());
                    
                return;
            }
            return;
        }
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
                "❌ Неверный формат. Пример: 2001-05-17");

            return;
        }
        if (message.Text == "/todo")
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);

            var items = await _todoService.GetByDayAsync(chatId, today);

            var text = _todoMessageBuilder.BuildDayText(items, today);
            var keyboard = _todoMessageBuilder.BuildDayKeyboard(today);

            await _telegramSender.SendTextAsync(chatId, text, keyboard);
        }
        if (user.State == UserState.WaitingTodoText)
        {
            await _todoService.AddAsync(chatId, userId, message.Text!);

            user.State = UserState.None;
            await _userService.UpdateAsync(user);

            await _telegramSender.SendTextAsync(chatId, "✅ Добавлено");

            return;
        }
    }
}