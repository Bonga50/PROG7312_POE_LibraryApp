using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PROG7312_POE_LibraryApp.Data;
using PROG7312_POE_LibraryApp.Models;
using System.Diagnostics;
using System.Reflection;

namespace PROG7312_POE_LibraryApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        DataAccess data = new DataAccess();

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ReplacingBook()
        {
            List<Books> model = data.getRandomnums(3);


            return View(model);
        }

        public IActionResult ReplaceBooksTutorial() {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult GetNewOrder(string order)
        {
            // Deserialize the JSON string into a list of strings
            List<string> mainNums = JsonConvert.DeserializeObject<List<string>>(order);

            // Process the order of items
            // ...

            return Json(new { success = true });
        }
    }
}