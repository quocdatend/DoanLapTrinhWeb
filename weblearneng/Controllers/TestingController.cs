using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace demotienganh.Controllers
{
    public class TestingController : Controller
    {
        private readonly ILogger<TestingController> _logger;
        DatawebengContext dbcontext = new DatawebengContext();

        public TestingController(ILogger<TestingController> logger)
        {
            _logger = logger;
        }
        public string checkuser()
        {
            string userid = HttpContext.Session.GetString("username");
            return userid;

        }
        public IActionResult Index()
        {
            string userid = checkuser();
            if (userid == null)
            {
                return RedirectToAction("Login", "Account");
            }
            else
            {
                var Exams = dbcontext.Exams.ToList();
                return View(Exams);
            }

        }

        public List<QuestionsContent> getQC(int idexam)
        {
            List<QuestionsContent> questionsContents = dbcontext.QuestionsContents.Where(p => p.Examid == idexam).ToList();
            for (int i = 0; i < questionsContents.Count; i++)
            {
                Console.WriteLine(questionsContents[i]);
            }
            return questionsContents;
        }
        public List<Question> getQestion(int idqc)
        {
            List<Question> questions = dbcontext.Questions.Where(p => p.Qcid == idqc).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine(questions[i]);
            }
            return questions;
        }
        public IActionResult DoTest(int id)
        {
            var Question_content = getQC((int)id);
            var Question = getQestion((int)id);
            return View(Question);
        }
        //so sanh ket qua 
        [HttpPost]
        public IActionResult GetUserResult(Dictionary<int, string> SelectedAnswers)
        {
            foreach (var item in SelectedAnswers)
            {
                int questionId = item.Key;
                string selectedAnswer = item.Value;
            }
            return RedirectToAction("Result");
        }
        public IActionResult UserResult()
        {
            return RedirectToAction("Result", "Testing");
        }
        [HttpPost]
        public IActionResult SubmitAnswers(Dictionary<int, string> SelectedAnswers)
        {
            foreach (var kvp in SelectedAnswers)
            {
                int questionId = kvp.Key;
                string selectedAnswer = kvp.Value;
                // Thực hiện xử lý dữ liệu ở đây
            }

            // Trả về view để hiển thị các câu trả lời đã chọn
            return View("SelectedAnswers", SelectedAnswers);
        }

        public IActionResult Result()
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