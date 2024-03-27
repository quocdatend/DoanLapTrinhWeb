using weblearneng.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace demotienganh.Controllers
{
    public class GrammarController : Controller
    {

        private readonly ILogger<GrammarController> _logger;

        DatawebengContext db = new DatawebengContext();
        public GrammarController(ILogger<GrammarController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
        var lstGrammar = db.Grammars.ToList();
         return View(lstGrammar);
         }

         public ActionResult CaseGrammar(string name)
        {
        var lstGrammar = db.Grammars.ToList();
        var lstNewGrammar = new List<Grammar>();
        foreach (var item in lstGrammar)
        {
            if (item.Name == name)
            {
                lstNewGrammar.Add(item);
            }
         }
        return View(lstNewGrammar);
         }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
