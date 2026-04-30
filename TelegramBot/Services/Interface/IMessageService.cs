using TelegramBot.Data.Entities;
using TelegramBot.Models;

namespace TelegramBot.Services.Interface;

public interface IMessageService
{
    Task SaveMessageAsync(ChatMessage message);
}