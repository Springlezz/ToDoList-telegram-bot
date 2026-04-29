using Microsoft.AspNetCore.Mvc;

namespace TelegramBot.Controllers;

public class MessageController : Controller
{
    // GET
    public IActionResult Index()
    {
        return View();
    }
}