using demotienganh.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace demotienganh.Controllers
{
    public class VocabularyController : Controller
    {
        private readonly ILogger<VocabularyController> _logger;

        HoctienganhContext db = new HoctienganhContext();
        public VocabularyController(ILogger<VocabularyController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Vocabulary()
        {
            var lstVocabulary = db.Vocabularies.ToList(); 
            return View(lstVocabulary);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
