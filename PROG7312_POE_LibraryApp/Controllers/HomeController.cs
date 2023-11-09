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
        List<Achivements> AchivementModel = AchievementDataHandler.Instance.achievements;
        private List<CallNumberNode> findingCallnumModel = DataAccess.Instance.findingCallNumberList;
        Dictionary<string,string> areaModel = new Dictionary<string,string>();
        int findCallnumberLevel = -1;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult identifyAreas()
        {
            if (areaModel.Count==0)
            {
                areaModel = DataAccess.Instance.getDeweyAreas();
            }
            
            return View(areaModel);
        }
        public IActionResult ReplacingBook()
        {
            if (!AchievementDataHandler.Instance.achievements[0].IsUnlocked)
            {
                // Store the current time in the session
                AchievementDataHandler.Instance.levelStartTime = DateTime.Now;
            }

            if (model.Count==0||model.Count<AchievementDataHandler.Instance.currentLevel) {
                model = DataAccess.Instance.getRandomnums(AchievementDataHandler.Instance.currentLevel);
            }

            return View(model);
        }

        public IActionResult findingCallNumbers()
        {
            if (findingCallnumModel.Count==0)
            {
                findingCallnumModel = DataAccess.Instance.getCallNumbersFromTextFile();

            }
            ViewBag.SelectedCallnumber = DataAccess.Instance.selectedRandomCallNumNode;
            ViewBag.WinsInRow = AchievementDataHandler.Instance.FindingCallNumberWins;
            ViewBag.HighScoreNum = AchievementDataHandler.Instance.FindingCallNumberHighScore;
            ViewBag.CurrentLevel = DataAccess.Instance.findingCallnumberLevel;
            return View(findingCallnumModel);
        
        }

        public IActionResult IdentifyAreasTutorial()
        {
            return View();
        }
        public IActionResult ReplaceBooksTutorial() {
            return View();
        }
        [HttpPost]
        public IActionResult findingCallNumbersNextLevel()
        {
            if (findCallnumberLevel != DataAccess.Instance.findingCallnumberLevel)
            {
                DataAccess.Instance.findingCallnumberLeveltracker = DataAccess.Instance.findingCallnumberLevel;
                findingCallnumModel = DataAccess.Instance.getCallNumbersFromTextFile();
            }
            ViewBag.SelectedCallnumber = DataAccess.Instance.selectedRandomCallNumNode;
            ViewBag.WinsInRow = AchievementDataHandler.Instance.FindingCallNumberWins;
            ViewBag.HighScoreNum = AchievementDataHandler.Instance.FindingCallNumberHighScore;
            ViewBag.CurrentLevel = DataAccess.Instance.findingCallnumberLevel;
            return View("findingCallNumbers", findingCallnumModel);
        }

            [HttpPost]
        public IActionResult goAgain() {
         
            areaModel = DataAccess.Instance.getDeweyAreas();
            
            return View("identifyAreas", areaModel);
        }
        [HttpPost]
        public IActionResult NextLevel()
        {
            ++AchievementDataHandler.Instance.currentLevel;
            if (model.Count!=AchievementDataHandler.Instance.currentLevel)
            {
                model = DataAccess.Instance.getRandomnums(AchievementDataHandler.Instance.currentLevel);
            }
            
            return View("ReplacingBook",model);
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
            bool result = false;
            if (data.Count==sorted.Count)
            {
                result = DataAccess.Instance.compareLists(sorted, data);
                
                //check speed achivement
                if (result)
                {
                    AchievementDataHandler.Instance.AddWin();
                    // Retrieve the start time from the session
                    DateTime startTimeStr = AchievementDataHandler.Instance.levelStartTime;
                    if (startTimeStr != null)
                    {
                        TimeSpan elapsed = DateTime.Now - startTimeStr;

                        // Check if less than 7 seconds have passed
                        if (elapsed.TotalSeconds < 7)
                        {
                            AchievementDataHandler.Instance.UnlockSpeedAchivement();
                        }

                    }
                }
                else {
                    AchievementDataHandler.Instance.AddLosses();
                }
            }
            else
            {
                AchievementDataHandler.Instance.AddLosses();
            }
            //check secret achivement
            if (AchievementDataHandler.Instance.numberOfLosses > 20)
            {
                AchievementDataHandler.Instance.UnlockLosses();
            }
            //check one punch achievement

            if (AchievementDataHandler.Instance.numberOfLosses==0&& AchievementDataHandler.Instance.currentLevel==10)
            {
                AchievementDataHandler.Instance.UnlockOnePunch();
            }
            //check King of the pirates
            if (AchievementDataHandler.Instance.currentLevel==10)
            {
                AchievementDataHandler.Instance.UnlockKing();
            }
            //check Mangekyo
            if (AchievementDataHandler.Instance.winStreak>=5)
            {
                AchievementDataHandler.Instance.UnlockMangekyo();
            }
            if (AchievementDataHandler.Instance.numberOfColMatchWins > 1 &&
               AchievementDataHandler.Instance.FindingCallNumberWins > 1 &&
               AchievementDataHandler.Instance.numberOfWins > 1)
            {
                AchievementDataHandler.Instance.UnlockHighScore();
            }
            return Json(new { success = result });
        }

        [HttpPost]
        public IActionResult checkIdentfiedArea([FromBody] Dictionary<string, string> data) {
            bool result = DataAccess.Instance.checkIdentifiedAreas(data);

            if (result) { AchievementDataHandler.Instance.AddColMatchWin(); }
            else { AchievementDataHandler.Instance.AddColMatchloose(); }
            //check infinite memory
            if (AchievementDataHandler.Instance.numberOfColMatchWins >=3)
            {
                AchievementDataHandler.Instance.UnlockInfiniteMemory();
            }
            //check Thinking hard
            if (AchievementDataHandler.Instance.numberOfColMatchLooses >= 5)
            {
                AchievementDataHandler.Instance.UnlockThinkingHard();
            }

            if (AchievementDataHandler.Instance.numberOfColMatchWins > 1 &&
               AchievementDataHandler.Instance.FindingCallNumberWins > 1 &&
               AchievementDataHandler.Instance.numberOfWins > 1)
            {
                AchievementDataHandler.Instance.UnlockHighScore();
            }
            return Json(new { success = result });
        }

        [HttpPost]

        public IActionResult checkFindingCallNumbers(string selectedValue) {
            bool result = DataAccess.Instance.checkSelectedFoundNumber(selectedValue);
            if (result)
            {
                if (AchievementDataHandler.Instance.FindingNumLostPoint==0)
                {
                    AchievementDataHandler.Instance.AddFindingnumberWin();
                }
                else
                {
                    AchievementDataHandler.Instance.FindingNumLostPoint = 0;
                }
                findingCallnumModel = new List<CallNumberNode>();
            }
            else
            {
                AchievementDataHandler.Instance.AddFindingLosses();
                AchievementDataHandler.Instance.FindingNumLostPoint = 1;
                DataAccess.Instance.startAgain();
                findingCallnumModel = new List<CallNumberNode>();
            }

            if (AchievementDataHandler.Instance.numberOfColMatchWins >1 && 
                AchievementDataHandler.Instance.FindingCallNumberWins>1 &&
                AchievementDataHandler.Instance.numberOfWins>1)
            {
                AchievementDataHandler.Instance.UnlockHighScore();
            }
            if (AchievementDataHandler.Instance.FindingCallNumberHighScore>=10)
            {
                AchievementDataHandler.Instance.UnlockMoney();
            }

            return Json(new { success = result });
        }

    }
}