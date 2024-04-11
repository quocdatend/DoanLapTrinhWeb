using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Text;
using weblearneng.Models;
using X.PagedList;
using static Azure.Core.HttpHeader;
using System.Security.Cryptography;

namespace weblearneng.Controllers
{
    public class AdminController : Controller
    {
        private readonly ILogger<AdminController> _logger;
        DatawebengContext dbcontext = new DatawebengContext();
        public AdminController(ILogger<AdminController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        // Admin Vocabulary
        public IActionResult Vocabulary(int? page)
        {
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                int pageSize = 6;
                int pageNumber = page == null || pageSize < 1 ? 1 : page.Value;
                var lstnewVocabualry = dbcontext.Vocabularies.ToList();
                PagedList<Vocabulary> lstVocabulary = new PagedList<Vocabulary>(lstnewVocabualry, pageNumber, pageSize);
                return View(lstVocabulary);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AddVocabulary()
        {   
            return View();
        }

        [HttpPost]
        public IActionResult AddVocabulary(string fchar, string nameen, string namevn, string type, string title) 
        {
            
            if (nameen != null && namevn != null && type != null && fchar != null)
            {
                var checkNameen = dbcontext.Vocabularies.Where(x => x.Nameen == nameen).FirstOrDefault();
                if (checkNameen == null) 
                {
                    var newaVoca = new Vocabulary()
                    {
                        Fchar = fchar,
                        Nameen = nameen,
                        Namevn = namevn,
                        Type = type,
                        Title = title
                    };
                    dbcontext.Vocabularies.Add(newaVoca);
                    dbcontext.SaveChanges();
                    ViewBag.Success = "Save Successful!";
                } else if(fchar.Length > 1)
                {
                    ViewBag.Error = "Input 'First Char' must 1 value!";
                } else
                {
                    ViewBag.Error = "The word you entered already exists!";
                }
            }
            return View();
        }

        public IActionResult ViewVocabulary(string nameen)
        {
            var thisname = dbcontext.Vocabularies.Where(x => x.Nameen.Equals(nameen)).FirstOrDefault();
            return View(thisname);
        }

        public IActionResult EditVocabulary(string nameen)
        {
            if (TempData["data"] != null)
            {
                var newthisname = dbcontext.Vocabularies.Where(x => x.Nameen.Equals(TempData["data"])).FirstOrDefault();
                return View(newthisname);
            }
            var thisname = dbcontext.Vocabularies.Where(x => x.Nameen.Equals(nameen)).FirstOrDefault();
            return View(thisname);
        }

        [HttpPost]
        public IActionResult EditVocabularySQL(string nname,string fchar ,string nnameen, string namevn, string type, string title)
        {
            
            if (fchar.Length == 1)
            {
                if(title != null)
                {
                    var parameter = new[]
                    {
                        new SqlParameter("@nameen", nname),
                        new SqlParameter("@voca_fchar", fchar),
                        new SqlParameter("@voca_nameen", nnameen),
                        new SqlParameter("@voca_namevn", namevn),
                        new SqlParameter("@voca_type", type),
                        new SqlParameter("@voca_title", title)
                    };
                    dbcontext.Database.ExecuteSqlRaw("EditVocabulary @nameen, @voca_fchar, @voca_nameen, @voca_namevn, @voca_type, @voca_title", parameter);
                } else
                {
                    var parameter = new[]
                    {
                        new SqlParameter("@nameen", nname),
                        new SqlParameter("@voca_fchar", fchar),
                        new SqlParameter("@voca_nameen", nnameen),
                        new SqlParameter("@voca_namevn", namevn),
                        new SqlParameter("@voca_type", type)
                    };
                    dbcontext.Database.ExecuteSqlRaw("EditVocabularyIfTitleNull @nameen, @voca_fchar, @voca_nameen, @voca_namevn, @voca_type", parameter);
                }
                TempData["Success"] = "Save Successful!";
            } else
            {
                TempData["Error"] = "Input First Char must 1 value!";
            }
            TempData["data"] = nnameen;
            return RedirectToAction("EditVocabulary");
        }

        public IActionResult DeleteVocabulary(string nameen)
        {
            var checkNameen = dbcontext.Vocabularies.Where(x => x.Nameen == nameen).FirstOrDefault();
            dbcontext.Vocabularies.Remove(checkNameen);
            dbcontext.SaveChanges();
            TempData["page"] = 1;
            return RedirectToAction("Vocabulary", "Admin");
        }

        public IActionResult DeleteVocabularyView(string nameen) 
        {
            var thisname = dbcontext.Vocabularies.Where(x => x.Nameen.Equals(nameen)).FirstOrDefault();
            return View(thisname);
        }

        // Admin testing

        //Exam action
        public IActionResult Exam()
        {
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                //làm code lấy các exam 
                var Exams = dbcontext.Exams.ToList();
                return View(Exams);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AddExam(Exam model) // Sử dụng [Bind] để chỉ định các trường được gửi từ form
        {
            string examName = model.Examname;
            if (examName != null)
            {
                Exam exams = new Exam { Examname = examName };
                using (var context = new DatawebengContext())
                {
                    context.Exams.Add(exams);
                    context.SaveChanges();
                    return RedirectToAction("Exam", "Admin");
                }
            }
            return View();
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exam = await dbcontext.Exams.FirstOrDefaultAsync(m => m.Examid == id);
            if (exam == null)
            {
                return NotFound();
            }

            return View(exam);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exam = await dbcontext.Exams.FindAsync(id);
            var questioncontents = dbcontext.QuestionsContents.Where(p => p.Examid == id).ToList();

            foreach (var item in questioncontents)
            {
                var questions = dbcontext.Questions.Where(p => p.Qcid == item.Contentid).ToList();
                foreach (var item2 in questions)
                {
                    dbcontext.Remove(item2);
                }
                dbcontext.Remove(item);
            }
            if (exam == null)
            {
                return NotFound();
            }


            dbcontext.Exams.Remove(exam);
            await dbcontext.SaveChangesAsync();

            // Redirect to the exam list view with a success message (optional)
            return RedirectToAction("Exam", "Admin");
        }
        // POST: Exam/EditExam/5
        public async Task<IActionResult> EditExam([Bind("Examid,Examname")] Exam exam, int? id)
        {
            var tmpe = await dbcontext.Exams.FindAsync(id);
            if (exam.Examname != null)
            {
                try
                {
                    using (var context = new DatawebengContext())
                    {
                        exam.Examid = id ?? 0;
                        context.Update(exam);
                        context.SaveChanges();
                    }
                    // Cập nhật thông tin của bài kiểm tra trong cơ sở dữ liệu
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExamExists(exam.Examid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(tmpe);
        }

        private bool ExamExists(int id)
        {
            return dbcontext.Exams.Any(e => e.Examid == id);
        }


        // Question_content ACCTION

        public IActionResult Question_content(int id)
        {
            HttpContext.Session.SetInt32("idexam", id);
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", id);
            if (role != null && role == "True")
            {
                var Questioncontents = dbcontext.QuestionsContents.Where(p => p.Examid == id).ToList();
                ViewBag.id = id;
                return View(Questioncontents);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AddQuestioncontent(QuestionsContent questionsContent, int id)
        {
            string role = HttpContext.Session.GetString("Role");
            if (questionsContent.TextContent != null)
            {
                using (var context = new DatawebengContext())
                {
                    questionsContent.Examid = id;
                    context.QuestionsContents.AddRange(questionsContent);
                    context.SaveChanges();
                    return RedirectToAction("Question_content", "Admin", new { id = id });
                }
            }
            ViewBag.idexam = id;
            HttpContext.Session.SetInt32("idexam", id);
            return View();
        }


        public async Task<IActionResult> DeleteQuestioncontent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var QC = await dbcontext.QuestionsContents.FirstOrDefaultAsync(m => m.Contentid == id);
            if (QC == null)
            {
                return NotFound();
            }
            return View(QC);
        }
        [HttpPost, ActionName("DeleteQuestioncontent")]
        public async Task<IActionResult> DeleteConfirmedQuestioncontent(int id)
        {
            var QC = await dbcontext.QuestionsContents.FindAsync(id);
            int idexam = QC.Examid ?? 0;
            var questions = dbcontext.Questions.Where(p => p.Qcid == id).ToList();
            if (QC == null)
            {
                return NotFound();
            }
            foreach (var item in questions)
            {
                dbcontext.Remove(item);
            }
            dbcontext.QuestionsContents.Remove(QC);
            await dbcontext.SaveChangesAsync();
            // Redirect to the exam list view with a success message (optional)
            return RedirectToAction("Question_content", "Admin", new { id = idexam });
        }

        public async Task<IActionResult> EditQuestionContent([Bind("QuestionsStyle,Picture,TextContent,Adudi,TextQuestionsbigIfhave")] QuestionsContent questionsContent, int? id)
        {
            var tmpe = await dbcontext.QuestionsContents.FindAsync(id);
            questionsContent.Examid = tmpe.Examid;
            if (questionsContent.TextContent != null)
            {
                try
                {
                    using (var context = new DatawebengContext())
                    {
                        questionsContent.Contentid = id ?? 0;
                        context.Update(questionsContent);
                        context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsContentExists(questionsContent.Contentid))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Question_content", "Admin", new { id = tmpe.Examid });
            }
            return View(tmpe);
        }
        private bool QuestionsContentExists(int id)
        {
            return dbcontext.QuestionsContents.Any(e => e.Contentid == id);
        }


        //Question


        public IActionResult Question(int id)
        {
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                HttpContext.Session.SetInt32("idexam", id);
                var questions = dbcontext.Questions.Where(p => p.Qcid == id).ToList();
                ViewBag.Idquestioncontent = id;
                return View(questions);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public IActionResult AddQuestion(Question question, int? id)
        {
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                if (question.CorrectAnswer != null)
                {
                    question.Qcid = id ?? 1;
                    using (var context = new DatawebengContext())
                    {
                        context.Questions.Add(question);
                        context.SaveChanges();
                        return RedirectToAction("Question", "Admin", new { id = id });
                    }
                }
                ViewBag.Idquestioncontent = id;
                return View();
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }
        public async Task<IActionResult> DeleteQuestion(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await dbcontext.Questions.FirstOrDefaultAsync(m => m.QuestionId == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }
        [HttpPost, ActionName("DeleteQuestion")]
        public async Task<IActionResult> DeleteConfirmedQuestion(int id, int idQ)
        {
            var question = await dbcontext.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }
            dbcontext.Questions.Remove(question);
            await dbcontext.SaveChangesAsync();
            // Redirect to the exam list view with a success message (optional)
            return RedirectToAction("Question", "Admin", new { id = idQ });
        }
        public async Task<IActionResult> EditQuestion([Bind("QuestionId,Qcid,QuestionText,AnswerA,AnswerB,AnswerC,AnswerD,CorrectAnswer")] Question question, int? id)
        {
            var tmpe = await dbcontext.Questions.FindAsync(id);
            question.Qcid = tmpe.Qcid;
            if (question.CorrectAnswer != null)
            {
                try
                {
                    using (var context = new DatawebengContext())
                    {
                        question.QuestionId = id ?? 0;
                        context.Update(question);
                        context.SaveChanges();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionsExists(question.QuestionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Question", "Admin", new { id = tmpe.Qcid });
            }
            return View(tmpe);
        }
        private bool QuestionsExists(int id)
        {
            return dbcontext.Questions.Any(e => e.QuestionId == id);
        }

        //Admin Grammar
        public IActionResult Grammar(int? page)
        {
            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                int pageSize = 6;
                int pageNumber = page == null || pageSize < 1 ? 1 : page.Value;
                var lstnewGrammar = dbcontext.Grammars.ToList();
                PagedList<Grammar> lstGrammar = new PagedList<Grammar>(lstnewGrammar, pageNumber, pageSize);
                return View(lstGrammar);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AddGrammar()
        {

            return View();
        }

        [HttpPost]
        public IActionResult AddGrammar(string name,string title,string confirm,string doubt,string negative,string order)
        {

            if (name != null && title != null)
            {
                var checkNameen = dbcontext.Grammars.Where(x => x.Name == name).FirstOrDefault();
                if (checkNameen == null)
                {
                    var newaVoca = new Grammar()
                    {
                        Name = name,
                        Title = title,
                        Confirm = confirm,
                        Doubt = doubt,
                        Negative = negative,
                        Order = order
                    };
                    dbcontext.Grammars.Add(newaVoca);
                    dbcontext.SaveChanges();
                    ViewBag.Success = "Save Successful!";
                }else
                {
                    ViewBag.Error = "The word you entered already exists!";
                }
            }
            return View();
        }

        public IActionResult ViewGrammar(string name)
        {
            var thisname = dbcontext.Grammars.Where(x => x.Name.Equals(name)).FirstOrDefault();
            return View(thisname);
        }

		public IActionResult EditGrammar(string name)
		{
			if (TempData["datagrammar"] != null)
			{
				var newthisname = dbcontext.Grammars.Where(x => x.Name.Equals(TempData["datagrammar"])).FirstOrDefault();
				return View(newthisname);
			}
            var thisname = dbcontext.Grammars.Where(x => x.Name.Equals(name)).FirstOrDefault();
			return View(thisname);
		}

		[HttpPost]
		public IActionResult EditGrammarSQL(string nname, string name, string title, string? confirm, string? doubt, string? negative, string? order)
		{
            ViewBag.isTrue = false;
            if (order != null && confirm == null && doubt == null && negative == null)
			{
				var parameter = new[]
				{
					new SqlParameter("@name", nname),
					new SqlParameter("@grammar_name", name),
					new SqlParameter("@grammar_title", title),
                    new SqlParameter("@grammar_order", order)
				};
                ViewBag.isTrue = true;
				dbcontext.Database.ExecuteSqlRaw("EditGrammarWithOrder @name, @grammar_name, @grammar_title, @grammar_order", parameter);
                TempData["Successgrammar"] = "Save Successful!";
            } else if(order == null && confirm != null && doubt != null && negative != null)
            {
                var parameter = new[]
                {
                    new SqlParameter("@name", nname),
                    new SqlParameter("@grammar_name", name),
                    new SqlParameter("@grammar_title", title),
                    new SqlParameter("@grammar_confirm", confirm),
                    new SqlParameter("@grammar_doubt", doubt),
                    new SqlParameter("@grammar_negative", negative)
                };
                ViewBag.isTrue = true;
                dbcontext.Database.ExecuteSqlRaw("EditGrammarWithOrderNull @name, @grammar_name, @grammar_title, @grammar_confirm, @grammar_doubt, @grammar_negative", parameter);
                TempData["Successgrammar"] = "Save Successful!";
            } else
            {
                TempData["Errorgrammar"] = "Just input OrderTense is Null and input Negative, Doublt, Confirm not Null OR vice versa";
            }
            if(ViewBag.isTrue)
            {
                TempData["datagrammar"] = name;
            } else
            {
                TempData["datagrammar"] = nname;
            }
			return RedirectToAction("EditGrammar");
		}

        public IActionResult DeleteGrammar(string name)
        {
            var checkNameGrEx = dbcontext.Testgrammars.Where(x => x.Name == name).FirstOrDefault();
            dbcontext.Testgrammars.Remove(checkNameGrEx);
            dbcontext.SaveChanges();
            var checkNameGr = dbcontext.Grammars.Where(x => x.Name == name).FirstOrDefault();
            dbcontext.Grammars.Remove(checkNameGr);
            dbcontext.SaveChanges();
            TempData["page"] = 1;
            return RedirectToAction("Grammar", "Admin");
        }

        public IActionResult DeleteGrammarView(string name)
        {
            var thisname = dbcontext.Grammars.Where(x => x.Name.Equals(name)).FirstOrDefault();
            return View(thisname);
        }
            // Grammar Example
        public IActionResult ViewGrammarExample(string name)
        {
            TempData["nameViewGrammarExample"] = name;
            var lstExample = dbcontext.Testgrammars.Where(x => x.Name == name).ToList();
            if(TempData["nameDeleteGraEx"]!= null)
            {
                var lstExampleDelete = dbcontext.Testgrammars.Where(x => x.Name == TempData["nameDeleteGraEx"]).ToList();
                return View(lstExampleDelete);
            }
            return View(lstExample);
        }

        public IActionResult AddGrammarExample()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddGrammarExample(string name, string exor, string ex1, string ex2, string ex3)
        {

            if (name != null)
            {
                if (exor == null && ex1 != null && ex2 != null && ex3 != null)
                {
                    var newaVoca = new Testgrammar()
                    {
                        Name = name,
                        Ex1 = ex1,
                        Ex2 = ex2,
                        Ex3 = ex3
                    };
                    dbcontext.Testgrammars.Add(newaVoca);
                    dbcontext.SaveChanges();
                    ViewBag.Success = "Save Successful!";
                }
                else if(exor != null && ex1 == null && ex2 == null && ex3 == null)
                {
                    var newaVoca = new Testgrammar()
                    {
                        Name = name,
                        ExOr = exor
                    };
                    dbcontext.Testgrammars.Add(newaVoca);
                    dbcontext.SaveChanges();
                    ViewBag.Success = "Save Successful!";
                } else
                {
                    ViewBag.Error = "Just input Exor Null and ex1,ex2,ex3 not Null OR vice versa";
                }
            }
            TempData["nameViewGrammarExample"] = name;
            return View();
        }

        public IActionResult DeleteGrammarExample(string name, int id)
        {
            TempData["nameViewGrammarExample"] = name;
            var checkNameen = dbcontext.Testgrammars.Where(x => x.Name == name && x.Id == id).FirstOrDefault();
            dbcontext.Testgrammars.Remove(checkNameen);
            dbcontext.SaveChanges();
            TempData["nameDeleteGraEx"] = name;
            return RedirectToAction("ViewGrammarExample", "Admin");
        }


        //Admin User
        public IActionResult UserAccount(int? page)
        {

            string role = HttpContext.Session.GetString("Role");
            _logger.LogInformation("Role retrieved from Session: {Role}", role);
            if (role != null && role == "True")
            {
                int pageSize = 6;
                int pageNumber = page == null || pageSize < 1 ? 1 : page.Value;
                var checkuser = dbcontext.Accounts.Where(x => x.Role == false).ToList();
                PagedList<Account> lstVocabulary = new PagedList<Account>(checkuser, pageNumber, pageSize);
                return View(lstVocabulary);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public IActionResult AddUserAccount()
        {
            return View();
        }

        [HttpPost]
        public IActionResult AddUserAccount(string name, string email, string pass)
        {
            var checkuser = dbcontext.Accounts.Where(x => x.Email == email).FirstOrDefault();
            if (checkuser == null)
            {
                string passhash = CalculateMD5(pass);
                var newaVoca = new Account()
                {
                    Name = name,
                    Email = email,
                    Role = false,
                    Pass = passhash
                };
                dbcontext.Accounts.Add(newaVoca);
                dbcontext.SaveChanges();
                ViewBag.Success = "Save Successful!";
            } else
            {
                ViewBag.Error = "Email is aready!";
            }
            ViewBag.Name = name;
            ViewBag.Pass = pass;
            return View();
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

        public IActionResult DeleteUserAccount(string name, string email)
        {
            var checkuser = dbcontext.Accounts.Where(x => x.Name == name && x.Email == email).FirstOrDefault();
            dbcontext.Accounts.Remove(checkuser);
            dbcontext.SaveChanges();
            return RedirectToAction("UserAccount", "Admin");
        }

        public IActionResult DeleteUserAccountView(string name, string email)
        {
            var checkuser = dbcontext.Accounts.Where(x => x.Name == name && x.Email == email).FirstOrDefault();
            return View(checkuser);
        }
    }
}
