using PROG7312_POE_LibraryApp.Models;

namespace PROG7312_POE_LibraryApp.Data
{
    public class AchievementDataHandler
    {

        private static AchievementDataHandler _instance = null;
        private static readonly object padlock = new object();
        public int currentLevel { get; set; } = 3;
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
        public int numberOfWins { get; set; }
        public int winStreak { get; set; }
        public int numberOfColMatchWins { get; set; }
        public int numberOfColMatchLooses { get; set; }
        public int numberOfLosses { get; set; }
        public DateTime levelStartTime { get; set; }
        public List<Achivements> achievements { get; set; } = new List<Achivements>() {
        new Achivements(101,"I am Speed","You are faster than light itself.","~/images/speed_achivment_logo.png",
        "Finish ordering in less than 7 seconds.",false,false),
        new Achivements(102,"Mangekyō Sharingan","Your sight goes beyond that of a mere mortal.","~/images/shuringun_achivment_logo.png",
        "Get the order perfect five times in a row.",false,false),
        new Achivements(103,"One punch is all it takes", "Your power goes even beyond your wildest expectations.","~/images/OnePunch_Achivement_Logo.png",
        "Solve the hardest level perfectly the first time.",false,false),
        new Achivements(104,"King of the Pirates","The determination you show is admirable and you should be proud of how far you have come.",
        "~/images/KingOfthePirate_Achivment_logo.png","Solve all the levels from easy to hard.",false,false),
        new Achivements(105,"Infinite Memory","You’ve got a memory so sharp, you could out-remember a dolphin! You’re one ‘fin-flipping’ memory maestro that can leave anyone ‘ocean-deep’ in awe!",
        "~/images/BigBrain3.jpg","Get the item matching right 3 times in a row",false,false),
        new Achivements(106,"Thinking hard","You dont give up easily and you will achive what ever you put your mind too.",
        "~/images/math-thinking.gif","Loose item column matching five times.",false,false),
        new Achivements(107,"?????","????????????????????????","~/images/darkWoejack_Achivement_logo.png","Lose the same level 20 times.",true,false)
        };

        public List<Achivements> GetAchivements()
        {
            return GetAchivements();
        }

        public void UnlockSpeedAchivement() {
            achievements.First(x => x.ID==101).IsUnlocked = true;
        }

        public void UnlockMangekyo()
        {
            achievements.First(x => x.ID == 102).IsUnlocked = true;
        }
        public void UnlockOnePunch() {
            achievements.First(x => x.ID == 103).IsUnlocked = true;
        }
        public void UnlockKing()
        {
            achievements.First(x => x.ID == 104).IsUnlocked = true;
        }
        public void UnlockLosses()
        {
            achievements.First(x => x.ID == 107).IsUnlocked = true;
            achievements.First(x => x.ID == 107).IsSecret = false;
        }

        public void AddWin() {
            this.numberOfWins = numberOfWins+1;
            this.winStreak = winStreak+1;
            this.numberOfLosses = 0;
        }
        public void AddLosses()
        {   
            this.numberOfLosses = numberOfLosses+1;
            this.winStreak = 0;
        }

        public void AddColMatchWin() {
            this.numberOfColMatchWins++;
            this.numberOfColMatchLooses = 0;
        }
        public void AddColMatchloose() {
            this.numberOfColMatchLooses++;
            this.numberOfColMatchWins = 0;
        }

        public void UnlockInfiniteMemory()
        {
            achievements.First(x => x.ID == 105).IsUnlocked = true;
        }
        public void UnlockThinkingHard()
        {
            achievements.First(x => x.ID == 106).IsUnlocked = true;
        }

    }
}
