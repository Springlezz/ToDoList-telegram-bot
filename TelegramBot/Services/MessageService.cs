using TelegramBot.Models;
using TelegramBot.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Data.Entities;
using TelegramBot.Models;
namespace TelegramBot.Services;

public class MessageService
{
    private readonly IMessageRepository _repository;

    public MessageService(IMessageRepository repository)
    {
        _repository = repository;
    }
    
    public async Task SaveMessageAsync(ChatMessage message)
    {
        var entity = new MessageEntity
        {
            MessageDate = message.MessageDate,
            ChatId = message.ChatId,
            TelegramUserId =  message.TelegramUserId,
            Username =  message.Username,
            MessageText = message.MessageText,
        };
        await _repository.AddAsync(entity);
    }
        
}