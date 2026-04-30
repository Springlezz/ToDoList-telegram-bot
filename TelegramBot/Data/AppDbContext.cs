using TelegramBot.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace TelegramBot.Data;

public class AppDbContext : DbContext
{ 
    public DbSet<UserEntity> Users { get; set; } = null!;
    public DbSet<MessageEntity> Messages { get; set; } = null!;
    public DbSet<ToDoItemEntity> ToDoItems { get; set; } = null!;
    
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
            
    }
    
}