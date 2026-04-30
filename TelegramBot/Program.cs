using TelegramBot.BackgroundServices;
using TelegramBot.Handlers;
using Microsoft.EntityFrameworkCore;
using TelegramBot.Data;
using TelegramBot.Scenario;
using TelegramBot.Services.Interface;
using TelegramBot.Repository;
using TelegramBot.Services;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using TelegramBot.Repository.Interface;
using TelegramBot.Handlers;

namespace TelegramBot;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        
        builder.Services.AddDbContext<AppDbContext>(options =>
            options.UseSqlite("Data Source=bot.db"));

        builder.Services.AddSingleton<ITelegramBotClient>(_ =>
            new TelegramBotClient(builder.Configuration["BotToken"]));
        
        builder.Services
            //.AddScoped<IMessageScenario, MessageScenario>()
            .AddScoped<ICallbackScenario, CallbackScenario>()
            .AddScoped<IMessageHandler, MessageHandler>()
            
        
            .AddScoped<IUpdateHandler, UpdateHandler>()
            .AddSingleton<ITodoMessageBuilder, TodoMessageBuilder>()
            .AddTransient<IUserService, UserService>()
          //  .AddTransient<IMessageService, MessageService>()
            .AddTransient<ITodoService, TodoService>();
        
        builder.Services.AddScoped<ICallbackHandler, CallbackHandler>();
        Console.WriteLine(typeof(MessageScenario).FullName);
        Console.WriteLine(typeof(IMessageScenario).FullName);
        
        builder.Services.AddScoped<IUserRepository, UserRepository>();
        builder.Services.AddScoped<ITodoRepository, TodoRepository>();
        builder.Services.AddScoped<IMessageRepository, MessageRepository>();
        builder.Services.AddScoped<IMessageScenario, MessageScenario>();
        
        builder.Services.AddTransient<ITelegramSender, TelegramSender>();
        
        builder.Services.AddHostedService<TelegramPollingService>();
        builder.Services.AddHostedService<BirthdayReminderService>();
        builder.Services.AddControllers();
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}