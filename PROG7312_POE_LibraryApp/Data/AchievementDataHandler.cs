using PROG7312_POE_LibraryApp.Models;

namespace PROG7312_POE_LibraryApp.Data
{
    public class AchievementDataHandler
    {

        private static AchievementDataHandler _instance = null;
        private static readonly object padlock = new object();

        AchievementDataHandler()
        {
        }

        public static AchievementDataHandler Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new AchievementDataHandler();
                    }
                    return _instance;
                }
            }
        }
        public List<Achivements> Achivements { get; set; } = new List<Achivements>() {
            new Achivements(101,"I am Speed","You are faster than light it's self.","~/images/speed_achivment_logo.png",
                "Finish ordering in less than 7 secconds.",false,false),
            new Achivements(102,"Mangekyō Sharingan","Your sight goes beyond that of a mere mortal.","~/images/shuringun_achivment_logo.png",
                "Get the order perfect five times in a row.",false,false),
            new Achivements(103,"One puch is all it takes", "Your power goes even beyond you wildest expectations.","~/images/OnePunch_Achivement_Logo.png",
                "Solve the hardest level first perfectly first time.",false,false),
            new Achivements(104,"King of the Pirates","The determination you show is admirable and you should be proud of how far you have come.",
                "~/images/KingOfthePirate_Achivment_logo.png","Solve all the levels from easy to hard",false,false),
            new Achivements(105,"?????","????????????????????????","~/images/darkWoejack_Achivement_logo.png","Loose the same level 20 times",true,false)
        };

        public List<Achivements> GetAchivements()
        {
            return GetAchivements();
        }

       
        
    }
}
