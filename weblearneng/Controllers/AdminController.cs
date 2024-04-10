using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using weblearneng.Models;
using X.PagedList;

namespace weblearneng.Controllers
{
    public class AdminController : Controller
    {
        DatawebengContext db = new DatawebengContext();
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Vocabulary(int? page)
        {
            int pageSize = 6;
            int pageNumber = page == null || pageSize < 1 ? 1 : page.Value;
            var lstnewVocabualry = db.Vocabularies.ToList();
            PagedList<Vocabulary> lstVocabulary = new PagedList<Vocabulary>(lstnewVocabualry, pageNumber, pageSize);
            return View(lstVocabulary);
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
                var checkNameen = db.Vocabularies.Where(x => x.Nameen == nameen).FirstOrDefault();
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
                    db.Vocabularies.Add(newaVoca);
                    db.SaveChanges();
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
            var thisname = db.Vocabularies.Where(x => x.Nameen.Equals(nameen)).FirstOrDefault();
            return View(thisname);
        }

        public IActionResult EditVocabulary(string nameen)
        {
            if (TempData["data"] != null)
            {
                var newthisname = db.Vocabularies.Where(x => x.Nameen.Equals(TempData["data"])).FirstOrDefault();
                return View(newthisname);
            }
            var thisname = db.Vocabularies.Where(x => x.Nameen.Equals(nameen)).FirstOrDefault();
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
                    db.Database.ExecuteSqlRaw("EditVocabualry @nameen, @voca_fchar, @voca_nameen, @voca_namevn, @voca_type, @voca_title", parameter);
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
                    db.Database.ExecuteSqlRaw("EditVocabularyIfTitleNull @nameen, @voca_fchar, @voca_nameen, @voca_namevn, @voca_type", parameter);
                }
                TempData["Success"] = "Save Successful!";
            } else
            {
                TempData["Error"] = "Input First Char must 1 value!";
            }
            var thisname = db.Vocabularies.Where(x => x.Nameen.Equals(nnameen)).FirstOrDefault();
            TempData["data"] = nnameen;
            return RedirectToAction("EditVocabulary");
        }

        public IActionResult DeleteVocabulary(string nameen)
        {
            var checkNameen = db.Vocabularies.Where(x => x.Nameen == nameen).FirstOrDefault();
            db.Vocabularies.Remove(checkNameen);
            db.SaveChanges();
            TempData["page"] = 1;
            return RedirectToAction("Vocabulary", "Admin");
        }

        public IActionResult DeleteVocabularyView(string nameen) 
        {
            var thisname = db.Vocabularies.Where(x => x.Nameen.Equals(nameen)).FirstOrDefault();
            return View(thisname);
        }
    }
}
