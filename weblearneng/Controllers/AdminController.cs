using Microsoft.AspNetCore.Mvc;

namespace weblearneng.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
