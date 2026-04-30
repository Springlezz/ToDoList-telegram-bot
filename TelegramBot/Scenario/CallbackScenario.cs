using TelegramBot.Services.Interface;
using TelegramBot.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Data.Entities;

namespace TelegramBot.Scenario;

public class CallbackScenario : ICallbackScenario
{
    private readonly ITodoService _todoService;
    private readonly ITodoMessageBuilder _builder;
    private readonly ITelegramSender _sender;
    private readonly IUserService _userService;

    public CallbackScenario(
        ITodoService todoService,
        ITodoMessageBuilder builder,
        ITelegramSender sender,
        IUserService userService)
    {
        _todoService = todoService;
        _builder = builder;
        _sender = sender;
        _userService = userService;
    }

    public async Task HandleAsync(CallbackQuery callback)
    {
        var chatId = callback.Message!.Chat.Id;
        var data = callback.Data;
        var telegramUser = callback.From;

        var user = await _userService.GetOrCreateAsync(
            telegramUser.Id,
            telegramUser.Username);
        
        if (data!.StartsWith("todo_day:"))
        {
            var dateStr = data.Replace("todo_day:", "");
            var day = DateOnly.Parse(dateStr);

            var items = await _todoService.GetByDayAsync(chatId, day);

            var text = _builder.BuildDayText(items, day);
            var keyboard = _builder.BuildDayKeyboard(day);

            await _sender.EditMessageAsync(
                chatId,
                callback.Message.MessageId,
                text,
                keyboard);

            return;
        }
        
        if (data == "todo_create")
        {
            user.State = UserState.WaitingTodoText;
            await _userService.UpdateAsync(user);

            await _sender.AnswerCallbackAsync(callback.Id);

            await _sender.SendTextAsync(chatId, "Введите название задачи");

            return;
        }
        
        if (data!.StartsWith("todo_clear:"))
        {
            var dateStr = data.Replace("todo_clear:", "");
            var day = DateOnly.Parse(dateStr);

            await _todoService.ClearByDayAsync(chatId, day);

            await _sender.AnswerCallbackAsync(callback.Id);

            var items = await _todoService.GetByDayAsync(chatId, day);

            var text = _builder.BuildDayText(items, day);
            var keyboard = _builder.BuildDayKeyboard(day);

            await _sender.EditMessageAsync(chatId, callback.Message.MessageId, text, keyboard);

            return;
        }
        
        if (data!.StartsWith("todo_toggle:"))
        {
            var idStr = data.Replace("todo_toggle:", "");
            var todoId = int.Parse(idStr);

            await _todoService.ToggleAsync(todoId);

            await _sender.AnswerCallbackAsync(callback.Id);

            var day = DateOnly.FromDateTime(DateTime.UtcNow);

            var items = await _todoService.GetByDayAsync(chatId, day);

            var text = _builder.BuildDayText(items, day);
            var keyboard = _builder.BuildDayKeyboard(day);

            await _sender.EditMessageAsync(chatId, callback.Message.MessageId, text, keyboard);

            return;
        }

        if (data == "birthday_set")
        {
            await _sender.AnswerCallbackAsync(callback.Id);

            user.State = UserState.WaitingBirthday;
            await _userService.UpdateAsync(user);

            await _sender.SendTextAsync(chatId,
                "📅 Введите дату рождения в формате YYYY-MM-DD");

            return;
        }
    }
}