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
        
        private List<Books> model = DataAccess.Instance.randomNums;
        List<Achivements> AchivementModel = AchievementDataHandler.Instance.Achivements;
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
            if (model.Count==0) {
                model = DataAccess.Instance.getRandomnums(3);
            }

            return View(model);
        }

        public IActionResult ReplaceBooksTutorial() {
            return View();
        }
        public IActionResult GotoAchivements()
        {
            return View(AchivementModel);
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
        public IActionResult CheckOrder(List<string> data)
        {
            // Your code here
            List<Books>sorted = DataAccess.Instance.getSortednums(model);
            bool result = DataAccess.Instance.compareLists(sorted, data);
            return Json(new { success = result });
        }
    


    }
}