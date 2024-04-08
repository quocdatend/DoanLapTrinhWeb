using Microsoft.AspNetCore.Mvc;
using weblearneng.Models;

namespace weblearneng.Controllers
{
    public class AdminController : Controller
    {
        DatawebengContext db = new DatawebengContext();
        public IActionResult Index()
        {
            return View();
        }
    }
}
