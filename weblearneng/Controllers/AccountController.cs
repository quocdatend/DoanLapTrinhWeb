using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace demotienganh.Controllers
{
    public class AccountController : Controller
    {

        private readonly ILogger<AccountController> _logger;

        //       SqlConnection con = new SqlConnection("Data Source=QUOCDAT\\SQLEXPRESS;Initial Catalog=DATAWEBENG;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        DatawebengContext db = new DatawebengContext();
        public AccountController(ILogger<AccountController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            // Kiểm tra tên đăng nhập và mật khẩu
            var usercheck = db.Accounts.Where(x => x.Name.Equals(username) && x.Pass.Equals(password)).FirstOrDefault();

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

        public IActionResult Signup()
        {
            return View();
        }

        /*
                public IActionResult Signup(string name, string email, string pass, string re_pass)
                {
                    SqlCommand cmd = new SqlCommand("add_account_from_user", con);
                    Account lstAccount = new Account();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", lstAccount.Name);
                    cmd.Parameters.AddWithValue("@Email", lstAccount.Email);
                    cmd.Parameters.AddWithValue("@Pass", lstAccount.Pass);
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    return View();
                }
        */

        public IActionResult ForgetPass(string email)
        {
            var lstAccount = db.Accounts.ToList();
            foreach (var account in lstAccount)
            {
                if (account.Email == email)
                {
                    ViewBag.Email = email;
                }
            }
            if (ViewBag.Email == email && email != null)
            {
                return View("ResetPass", "Account");
            }
            ViewBag.Email = email;
            return View();
        }

        public IActionResult ResetPass(string checkpass, string password)
        {

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
