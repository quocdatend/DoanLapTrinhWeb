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

        public IActionResult ExampleGrammar(string name)
        {
            var checkname = db.Testgrammars.Where(x => x.Name.Equals(name)).ToList();
            var grammar = db.Grammars.Where(x => x.Name.Equals(name)).FirstOrDefault();
            List<string> ls = new List<string>(); 
            if (grammar.Order == null)
            {
                foreach(var item in checkname)
                {
                    ls.Add(item.Ex1);
                    ls.Add(item.Ex2);
                    ls.Add(item.Ex3);
                }
                ViewBag.Confirm = grammar.Confirm;
                ViewBag.Doubt = grammar.Doubt;
                ViewBag.Negative = grammar.Negative;

			} else
            {
				foreach (var item in checkname)
				{
				    ls.Add(item.ExOr);
				}
			}
            ViewBag.Name = name;
            return View(ls);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
