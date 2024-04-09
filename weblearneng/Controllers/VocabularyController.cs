using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using X.PagedList;
using Microsoft.IdentityModel.Tokens;

namespace demotienganh.Controllers
{
    public class VocabularyController : Controller
    {
        private readonly ILogger<VocabularyController> _logger;

        DatawebengContext db = new DatawebengContext();
        public VocabularyController(ILogger<VocabularyController> logger)
        {
            _logger = logger;
        }

        //public IActionResult Index()
        //{
        //    return View();
        //}

        public IActionResult Search(string search, int? page)
        {
            int pageSize = 2;
            int pageNumber = page == null || pageSize <1 ? 1 : page.Value;
            var lstnewVocabualry = db.Vocabularies.Where(x => x.Nameen.Contains(search)).ToList();
            PagedList<Vocabulary> lst = new PagedList<Vocabulary>(lstnewVocabualry, pageNumber, pageSize);
            ViewBag.Search = search;
            if (search.IsNullOrEmpty()) ViewBag.Null = "Bạn chưa nhập già trị nào!";
            else if (lstnewVocabualry.Count == 0 ) ViewBag.Null = "Giá trị nhập không tồn tại!";
			return View(lst);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
