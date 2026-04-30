using TelegramBot.Data.Entities;
using TelegramBot.Models;
namespace TelegramBot.Repository.Interface;

public interface IMessageRepository
{
    Task<MessageEntity?> GetByIdAsync(Guid id);
    Task AddAsync(MessageEntity message);
    Task UpdateAsync(MessageEntity message);
    Task DeleteAsync(MessageEntity message);
}