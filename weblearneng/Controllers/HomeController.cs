using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace demotienganh.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IEmailSender emailSender;
        public HomeController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }
        //public HomeController(ILogger<HomeController> logger)
        //{
        //    _logger = logger;
        //}

        public async Task<IActionResult> Index(string? name, string? email, string? message)
        {
            if (email != null && name != null && message != null)
            {
                await emailSender.SendEmailAsync("hnqdat2003@gmail.com", "Client Interaction", "Content: \n" + message + "\n" + "Name: " + name + "\nEmail: " + email);
            }
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}