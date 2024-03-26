using demotienganh.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
namespace demotienganh.Controllers
{
    public class AccountController : Controller
    {
        DatawebengContext context = new DatawebengContext();
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public IActionResult Login(string username, string password)
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            var usercheck = context.Accounts.Where(x => x.Name.Equals(username) && x.Pass.Equals(password)).FirstOrDefault();
            
            if (usercheck != null)
            {
                String userid = usercheck.Id.ToString();
                // Lưu tên đăng nhập trong session
                HttpContext.Session.SetString("username", userid);
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }
        }

        public IActionResult Logout()
        {
            // Xóa tên đăng nhập khỏi session khi đăng xuất
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }
    }
}
