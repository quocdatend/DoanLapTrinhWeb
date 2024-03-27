using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

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

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search(string search)
        {
            var lstVocabualry = db.Vocabularies.ToList();
            var lstnewVocabualry = new List<Vocabulary>();
            foreach(var item in  lstVocabualry)
            {
                if(search != null)
                {
                if(item.Nameen.Contains(search))
                    {
                        lstnewVocabualry.Add(item);
                    }
                }
                
            }
            return View(lstnewVocabualry);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
