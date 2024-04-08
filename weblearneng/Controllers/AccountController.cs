using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Runtime.Intrinsics.Arm;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Text;
using System.Security.Cryptography;

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

        DatawebengContext db = new DatawebengContext();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        { 
            string newhash = CalculateMD5(password);
            // Kiểm tra tên đăng nhập và mật khẩu
            var usercheck = db.Accounts.Where(x => x.Name.Equals(username) && x.Pass.Equals(newhash.Substring(0,12))).FirstOrDefault();
            if (usercheck != null)
            {
                
                String userid = usercheck.Id.ToString();
                HttpContext.Session.SetInt32("idAccount", usercheck.Id);
                // Lưu tên đăng nhập trong session
                HttpContext.Session.SetString("username", userid);
                if(usercheck.Role)
                {
                    HttpContext.Session.SetString("Role", usercheck.Role.ToString());
					return RedirectToAction("Index", "Admin");
				}
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.Error = "Tên đăng nhập hoặc mật khẩu không đúng.";
                return View();
            }
        }

        public string CalculateMD5(string input)
        {
            byte[] tmpSource = ASCIIEncoding.ASCII.GetBytes(input);

            //Compute hash based on source data
            byte[] tmpHash = new MD5CryptoServiceProvider().ComputeHash(tmpSource);
            int i;
            StringBuilder sOutput = new StringBuilder(tmpHash.Length);
            for (i = 0; i < tmpHash.Length - 1; i++)
            {
                sOutput.Append(tmpHash[i].ToString("X2"));
            }
            return sOutput.ToString();
        }

        public IActionResult Logout()
        {
            // Xóa tên đăng nhập khỏi session khi đăng xuất
            HttpContext.Session.Remove("username");
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Signup(string? name, string? email, string? pass, string? re_pass)
        {
            ViewBag.Error = null;
            ViewBag.Success = null;
            ViewBag.isTrue = true;        
            if(email != null && name != null && pass != null && re_pass != null)
            {
                if(pass != re_pass)
                {
                    ViewBag.Name = name;
                    ViewBag.Email = email;
                    ViewBag.Pass = pass;
                    ViewBag.Re_pass = re_pass;
                    ViewBag.Error = "Error: Password not same!";
                } else
                {
                    string repasshash = CalculateMD5(re_pass);
                    var newaccount = new Account()
                    {
                        Name = name,
                        Email = email,
                        Role = false,
                        Pass = repasshash,
                    };
                    db.Accounts.Add(newaccount);
                    db.SaveChanges();
                    ViewBag.Name = name;
                    ViewBag.Email = email;
                    ViewBag.Pass = pass;
                    ViewBag.Re_pass = re_pass;
                    ViewBag.isTrue = true;
                    ViewBag.Success = "Save Successful!";
                }
            }
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

        public ActionResult<IEnumerable<Account>> ResetPass(string? resetpass, string? confirmpass)
        {
            string email = HttpContext.Session.GetString("email");
            ViewBag.isTrue = false;
            var account = db.Accounts.SingleOrDefault(x => x.Email == email);          
            //bool isMatch = string.Equals(newHash, hashToCompare, StringComparison.OrdinalIgnoreCase);
            if (resetpass != confirmpass)
            {
                ViewBag.error = "Your input password not same!";
            } else if (resetpass != null)
            {
                var repasshash = CalculateMD5(resetpass);
                var parameter = new[]
                {
                    new SqlParameter("@account_id", account.Id),
                    new SqlParameter("@account_pass", repasshash)
                };
                db.Database.ExecuteSqlRaw("UpdateAccountInFGPass @account_id, @account_pass", parameter);
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
