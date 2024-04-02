using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Runtime.Intrinsics.Arm;

namespace demotienganh.Controllers
{
    public class AccountController : Controller
    {
        private readonly IEmailSender emailSender;
        private readonly ILogger<AccountController> _logger;
        public AccountController(IEmailSender emailSender)
        {
            this.emailSender = emailSender;
        }
        //       SqlConnection con = new SqlConnection("Data Source=QUOCDAT\\SQLEXPRESS;Initial Catalog=DATAWEBENG;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

        DatawebengContext db = new DatawebengContext();
      //  public AccountController(ILogger<AccountController> logger)
    //    {
  //          _logger = logger;
//        }

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

        public async Task<IActionResult> ForgetPass(string email)
        {
            ViewBag.isTrue = false;
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
                ViewBag.isTrue = true;
                HttpContext.Session.SetString("email", email);
                ForgetPass();
                return View();
            }
            ViewBag.Email = email;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgetPass()
        {
            TempData["code"] = RandomCode(); // not use this
            string newcode = RandomCode();
            HttpContext.Session.SetString("code", newcode);
            string email = HttpContext.Session.GetString("email");
            await emailSender.SendEmailAsync(email, "Code Reset Password", "Kính gửi," +
                "\r\n\r\nChúng tôi đã nhận được yêu cầu đặt lại mật khẩu từ tài khoản của bạn. Dưới đây là mã code bạn cần để tiếp tục quá trình đặt lại mật khẩu" +
                "\r\n\r\nMã Code: " + newcode + "" +
                "\r\n\r\nVui lòng sao chép mã code này và sử dụng nó trên trang đặt lại mật khẩu để hoàn tất quá trình." +
                "\r\n\r\nNếu bạn không yêu cầu đặt lại mật khẩu này, vui lòng bỏ qua email này. Tài khoản của bạn sẽ vẫn an toàn." +
                "\r\n\r\nĐảm bảo rằng bạn đặt mật khẩu mạnh sau khi đặt lại, bao gồm ít nhất 8 ký tự, bao gồm cả chữ hoa, chữ thường, số và ký tự đặc biệt." +
                "\r\n\r\nNếu bạn cần sự trợ giúp hoặc có bất kỳ câu hỏi nào, đừng ngần ngại liên hệ với chúng tôi qua email này." +
                "\r\n\r\nTrân trọng,");
            return RedirectToAction("EnterCode"); // and this
        }
        
        public IActionResult EnterCode(string? passcode, string code)
        {
            string newcode = HttpContext.Session.GetString("code");
            ViewBag.Code = newcode;

            if (passcode == newcode && passcode != null)
            {
                ViewBag.Passcode = newcode;
                ViewBag.Newcode = newcode;
            }
            
            if(passcode != null && passcode != newcode)
            {
                ViewBag.Error = "Nhap sai, vui long nhap lai voi code moi!";
            }
            return View();
        }

        public string RandomCode()
        {
            Random _rdm = new Random();
            int _min = 1000;
            int _max = 9999;
            return _rdm.Next(_min, _max).ToString();
        }

        public IActionResult ResetPass(string? resetpass, string? confirmpass)
        {
            ViewBag.isTrue = false;
            // su ly truy van csdl tai day
            if (resetpass != confirmpass)
            {
                ViewBag.error = "Your input password not same!";
            } else if(resetpass != null)
            {
                ViewBag.isTrue = true;
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
