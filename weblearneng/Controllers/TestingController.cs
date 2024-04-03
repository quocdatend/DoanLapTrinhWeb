using weblearneng.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using Newtonsoft.Json;
using weblearneng.ViewsModel;
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
        // lấy question context id bằng idexam
        public List<QuestionsContent> getQC(int idexam)
        {
            List<QuestionsContent> questionsContents = dbcontext.QuestionsContents.Where(p => p.Examid == idexam).ToList();
            for (int i = 0; i < questionsContents.Count; i++)
            {
                Console.WriteLine(questionsContents[i]);
            }
            return questionsContents;
        }
        //lấy danh sách câu hỏi bằng Question content  
        public List<Question> getQestion(int idqc)
        {
            List<Question> questions = dbcontext.Questions.Where(p => p.Qcid == idqc).ToList();
            for (int i = 0; i < questions.Count; i++)
            {
                Console.WriteLine(questions[i]);
            }
            return questions;
        }

        // lấy question_context . dựa vào idqc của từng question context lấy question id hiện thị lên views : sửa lại views
        public void getIdQByIdQC(int idexam)
        {
            List<Question> danhsachQ = new List<Question>();
            List<QuestionsContent> danhsachQC = getQC(idexam);
            foreach (var questionContent in danhsachQC)
            {
                danhsachQ = dbcontext.Questions.Where(p => p.Qcid.Equals(questionContent.Contentid)).ToList();
                foreach (var q in danhsachQ)
                {
                    _logger.LogInformation($"Question ID: {q.QuestionId}, Question-cid: {questionContent.Contentid}");
                }
            }
        }
        public List<Question> getQbyQCid(List<QuestionsContent> danhsachQC)
        {
            List<Question> danhsachQ = new List<Question>();
            List<Question> danhsachTemp = new List<Question>();
            foreach (var questionContent in danhsachQC)
            {
                _logger.LogInformation($"Question ID: {questionContent.Contentid}");
                danhsachTemp = dbcontext.Questions.Where(p => p.Qcid.Equals(questionContent.Contentid)).ToList();
                foreach (var question in danhsachTemp)
                {
                    danhsachQ.Add(question);
                }
            }
            return danhsachQ;
        }

        public IActionResult DoTest(int id)
        {
            HttpContext.Session.SetInt32("idexam", id);
            List<QuestionsContent> question_content = getQC(id);
            var question = getQbyQCid(question_content);
            ViewBag.QuestionContent = question_content;
            DateTime timebegin = DateTime.Now;

            string timeBeginString = timebegin.ToString("yyyy-MM-dd HH:mm:ss");
            _logger.LogInformation($"{timebegin}");
            HttpContext.Session.SetString("timebegin", timeBeginString);
            return View(question);

        }
        //so sanh ket qua 
        // nhận dữ liệu từ Dotest dua du lieu vao lop 
        [HttpPost]
        public IActionResult SubmitAnswers(Dictionary<int, string>? selectedAnswers, int id)
        {
            if (selectedAnswers != null)
            {
                // 2 trường hợp quan trọng
                int idexam = HttpContext.Session.GetInt32("idexam") ?? 0;
                List<Result> resultList = new List<Result>();
                List<QuestionsContent> question_content = getQC(idexam);
                var question = getQbyQCid(question_content);
                foreach (var questionItem in question)
                {

                    int idquestion1 = questionItem.QuestionId;
                    string TAnswer = questionItem.CorrectAnswer;
                    string selectedAnswer = null; // Khởi tạo selectedAnswer với giá trị mặc định là null

                    // Kiểm tra xem câu hỏi có trong selectedAnswers hay không
                    if (selectedAnswers.ContainsKey(idquestion1))
                    {
                        selectedAnswer = selectedAnswers[idquestion1]; // Lấy đáp án đã chọn từ selectedAnswers
                    }
                    Boolean Ketquacmp = GetFalseAnswers(selectedAnswer, TAnswer);
                    // Tạo một đối tượng Result và thêm vào danh sách resultList
                    Result result = new Result
                    {
                        QuestionId = idquestion1,
                        Answer = selectedAnswer, // Sử dụng selectedAnswer được xác định ở trên
                        TrueAnswer = TAnswer,
                        Ketqua = Ketquacmp
                    };
                    DateTime timeend = DateTime.Now;
                    string timeEndString = timeend.ToString("yyyy-MM-dd HH:mm:ss");
                    _logger.LogInformation($"{timeEndString}");
                    HttpContext.Session.SetString("timeend", timeEndString);
                    resultList.Add(result);
                    // Ghi log
                    // _logger.LogInformation($"Question ID: {idquestion1}, Selected Answer: {TAnswer}, True Answer: {selectedAnswer} , ketqua :{Ketquacmp}");
                }
                foreach (var item in resultList)
                {
                    _logger.LogInformation($"Question ID: {item.QuestionId}, Selected Answer: {item.TrueAnswer}, True Answer: {item.Answer} , ketqua :{item.Ketqua}");
                }
                TempData["ResultList"] = JsonConvert.SerializeObject(resultList);
                return RedirectToAction("Result", "Testing");
            }
            else
            {
                return RedirectToAction("DoTest", "Testing");
            }

        }
        // lấy theo exam sửa lại 
        //done
        //lấy đáp án idquestion theo exam lưu lại id exa

        public String GetTrueAnswers(int idquestion)
        {
            string Tanswer = dbcontext.Questions.Where(p => p.QuestionId == idquestion).Select(p => p.CorrectAnswer).FirstOrDefault();
            return Tanswer;
        }
        public Boolean GetFalseAnswers(string Submit, String Correct)
        {

            if (Submit != null)
            {
                if (Submit.Equals(Correct))
                {
                    return true;
                }
                return false;
            }
            return false;
        }
        //lấy đáp án người dùng theo question id và selectedAnswer

        // so sánh đáp án người dùng với đáp án đúng theo tạo ViewsDât -> tao result gồm id question ,selectedAnswer, CorrectAnswer, và check

        // tính điểm 

        // đưa vào database historyExam  

        //chỉnh lại kết quả viewss nếu 

        public int Count(List<Result> list)
        {
            int count = 0;
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Ketqua == true)
                {
                    count++;
                }
            }
            return count;
        }
        public int GetScore(List<Result> resultList)
        {
            int totalQuestions = resultList.Count;
            if (totalQuestions == 0)
            {
                return 0; // Trả về 0 nếu không có câu hỏi nào
            }
            int correctAnswers = 0;
            foreach (var result in resultList)
            {
                if (result.Ketqua == true)
                {
                    correctAnswers++;
                }
            }
            int score = (correctAnswers * 100) / totalQuestions;
            return score;

        }

        public IActionResult Result()
        {
            if (TempData["ResultList"] != null)
            {
                var serializedResultList = TempData["ResultList"].ToString();
                var resultList1 = JsonConvert.DeserializeObject<List<Result>>(serializedResultList);

                TempData["ResultList"] = JsonConvert.SerializeObject(resultList1);

                int CauDung = Count(resultList1);
                int Diem = GetScore(resultList1);

                ViewBag.CauDung = CauDung;
                ViewBag.Diem = Diem;
                return View(resultList1);
            }
            else
            {
                // Xử lý trường hợp TempData không có dữ liệu
                return RedirectToAction("Home", "Index");
            }
        }
        public IActionResult Exit()
        {
            // Thực hiện các xử lý cần thiết (nếu có)
            return RedirectToAction("Index", "Home");
        }
        public IActionResult ReDoTest()
        {
            // Thực hiện các xử lý cần thiết (nếu có)
            return RedirectToAction("Index", "Testing");
        }

        public Boolean luuKetqua()
        {
            var serializedResultList = TempData["ResultList"].ToString();
            var resultList1 = JsonConvert.DeserializeObject<List<Result>>(serializedResultList);
            int idaccount = HttpContext.Session.GetInt32("idAccount") ?? 0;
            int idexam = HttpContext.Session.GetInt32("idexam") ?? 0;
            int Diem = GetScore(resultList1);
            if (resultList1 != null)
            {
                List<UserResponse> ListUserResponse = new List<UserResponse> { };
                foreach (var item in resultList1)
                {
                    int cmpketqua = 0;
                    if (item.Ketqua == true)
                    {
                        cmpketqua = 1;
                    }
                    ListUserResponse.Add(new UserResponse { UserId = idaccount, ExamId = idexam, QuestionId = item.QuestionId, UserAnswer = item.Answer, IsCorrect = cmpketqua });
                }
                string begin = HttpContext.Session.GetString("timebegin");
                string end = HttpContext.Session.GetString("timeend");
                DateTime timebegin = DateTime.Parse(begin);
                DateTime timeend = DateTime.Parse(end);
                ExamHistory history = new ExamHistory
                {
                    UserId = idaccount,
                    ExamId = idexam,
                    StartTime = timebegin,
                    EndTime = timeend,
                    Score = Diem
                };
                _logger.LogInformation("ExamHistory object created - UserId: {UserId}, ExamId: {ExamId}, StartTime: {StartTime}, EndTime: {EndTime}, Score: {Score}",
        idaccount, idexam, timebegin, timeend, Diem);

                // Logging the creation
                using (var context = new DatawebengContext())
                {
                    context.UserResponses.AddRange(ListUserResponse);
                    context.ExamHistories.Add(history);
                    context.SaveChanges();
                    return true;
                }
            }
            //Mai lam
            return false;
        }

        public IActionResult Savetest()
        {
            if (luuKetqua())
            {
                ViewBag.AlertMessage = "Đã lưu thành công!";
            }
            else
            {
                ViewBag.AlertMessage = "Khong thành công!";
            }
            return RedirectToAction("Index", "Testing");
        }
    }
}