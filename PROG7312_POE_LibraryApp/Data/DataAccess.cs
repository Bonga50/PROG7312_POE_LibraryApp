using PROG7312_POE_LibraryApp.Models;
using System.Linq;

namespace PROG7312_POE_LibraryApp.Data
{
    public class DataAccess
    {

        private static DataAccess _instance = null;
        private static readonly object padlock = new object();
        public List<Books> randomNums = new List<Books>();
        public Dictionary<string, string> areaCallNums = new Dictionary<string, string>();
        public List<identifyArea> identifyAreaBooks = new List<identifyArea>();

        DataAccess()
        {
        }

        public static DataAccess Instance
        {
            get
            {
                lock (padlock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataAccess();
                    }
                    return _instance;
                }
            }
        }

        //List of Aurthors
        List<string> authors = new List<string>()
        {
            "John Smith", "Jane Austen", "Mark Twain", "Emily Bronte", "Charles Dickens",
            "William Shakespeare", "Virginia Woolf", "Ernest Hemingway", "Scott Fitzgerald", "George Orwell",
            "James Joyce", "Thomas Hardy", "Edgar Poe", "Robert Frost", "Eliot Cummings",
            "Arthur Doyle", "Agatha Christie", "Stephen King", "Rowling Potter", "Lewis Carroll"
        };

        List<string> existingSurnames = new List<string>();

        //Dictionary of call numbers and Areas
        Dictionary<string, string> identifyAreaList = new Dictionary<string, string>()
        {
            {"000", "Generalities"},
            {"100", "Philosophy & psychology"},
            {"200", "Religion"},
            {"300", "Social sciences"},
            {"400", "Language"},
            {"500", "Natural sciences & mathematics"},
            {"600", "Technology (Applied sciences)"},
            {"700", "The arts"},
            {"800", "Literature & rhetoric"},
            {"900", "Geography & history"}
        };

        /// <summary>
        /// Method will generate differnt authors initals 
        /// </summary>
        /// <returns></returns>
        public string getRandomSurnames()
        {
            bool exits = true;
            string surname = "";
            Random rnd = new Random();

            do
            {
                surname = GetSurname(authors[rnd.Next(0, authors.Count)]);
                surname = surname.Substring(0, 3).ToUpper();

                if (!existingSurnames.Contains(surname))
                {
                    exits = false;
                    existingSurnames.Add(surname);

                }


            } while (exits);
            return surname;

        }
        /// <summary>
        /// This method first splits the input string by spaces, assuming that the last part is the surname.
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public string GetSurname(string fullName)
        {
            string[] nameParts = fullName.Split(' ');
            if (nameParts.Length >= 2)
            {
                return nameParts[nameParts.Length - 1];
            }
            else
            {
                // Handle cases where there might not be a surname.
                // You can return an empty string or handle it as needed.
                return string.Empty;
            }
        }

        /// <summary>
        /// Method that will generate a library call number, returnms an object of the call number
        /// </summary>
        /// <returns></returns>
        public Books generateRandomNums()
        {
            Books rObj = new Books();

            string tempval = "";
            string temp2val = "";
            string surname = "";
            //generating main group
            for (int i = 0; i < 3; i++)
            {
                tempval = tempval + generateNum().ToString();
            }
            rObj.groupNum = tempval;
            //generating sub group
            for (int i = 0; i < 3; i++)
            {
                temp2val = temp2val + generateNum().ToString();
            }
            rObj.subGroupNum = temp2val;

            surname = getRandomSurnames();
            rObj.authirInitials = surname;

            rObj.mainNum = tempval + "." + temp2val + " " + surname;
            return rObj;
        }

        /// <summary>
        /// Method that wil generate a random number from 0 to 9
        /// </summary>
        /// <returns></returns>
        public int generateNum()
        {
            Random rand = new Random();

            return rand.Next(0, 9);
        }

        /// <summary>
        /// methord that will get the different generated call numbers and add them to a list, takes a integer that will define the size of the list
        /// </summary>
        /// <param name="size"></param>
        /// <returns></returns>
        public List<Books> getRandomnums(int size)
        {
            randomNums.Clear();
            existingSurnames.Clear();
            if (size > 10)
            {
                size = 10;
            }
            for (int i = 0; i < size; i++)
            {
                randomNums.Add(generateRandomNums());
            }

            return randomNums;
        }

        /// <summary>
        /// method that will returnn a sorted list of call numbers
        /// </summary>
        /// <param name="unSorted"></param>
        /// <returns></returns>
        public List<Books> getSortednums(List<Books> unSorted)
        {

            for (int i = 0; i < unSorted.Count - 1; i++)
            {
                for (int j = (i + 1); j < unSorted.Count; j++)
                {
                    //sort y group num
                    if (String.Compare(unSorted[j].groupNum, unSorted[i].groupNum) < 0)
                    {
                        Books temp = unSorted[j];
                        unSorted[j] = unSorted[i];
                        unSorted[i] = temp;
                    }

                    //sort sub group num
                    if (String.Compare(unSorted[j].groupNum, unSorted[i].groupNum) == 0)
                    {
                        if (String.Compare(unSorted[j].subGroupNum, unSorted[i].subGroupNum) < 0)
                        {
                            Books temp = unSorted[j];
                            unSorted[j] = unSorted[i];
                            unSorted[i] = temp;
                        }
                    }
                    //sort by author
                    if ((String.Compare(unSorted[j].groupNum, unSorted[i].groupNum) == 0)
                        && (String.Compare(unSorted[j].subGroupNum, unSorted[i].subGroupNum) == 0))
                    {
                        if (String.Compare(unSorted[j].authirInitials, unSorted[i].authirInitials) < 0)
                        {
                            Books temp = unSorted[j];
                            unSorted[j] = unSorted[i];
                            unSorted[i] = temp;
                        }

                    }


                }
            }
            return unSorted;
        }

        public bool compareLists(List<Books> sorted, List<string> usersList)
        {

            for (int i = 0; i < sorted.Count; i++)
            {
                if (sorted[i].mainNum != usersList[i])
                {
                    return false;
                }
            }
            return true;
        }

        public Dictionary<string, string> getDeweyAreas() {
            Dictionary<string, string> tempCallNums = new Dictionary<string, string>();
            List<string> tempNums = new List<string>();
            List<int> tempExisting = new List<int>();
            //adding call nums in random order
            int count = 0;
            while (count < 4) {
                Random rand = new Random();
                Random rand2 = new Random();
                int y = rand.Next(0, identifyAreaList.Count);
                int z = rand2.Next(0, 2);
                
                if (!tempNums.Contains(identifyAreaList.ElementAt(y).Key) && !tempNums.Contains(identifyAreaList.ElementAt(y).Value))
                {
                    if (z == 1)
                    {
                        tempNums.Add(identifyAreaList.ElementAt(y).Key);
                        count++;
                    }
                    else
                    {
                        tempNums.Add(identifyAreaList.ElementAt(y).Value);
                        count++;
                    }
                   
                }
            }
            tempCallNums = generateRandomAreas();
            count = 0;
            while (count < 4) {
                Random random = new Random();
                int mynum = random.Next(0, tempCallNums.Count);

                if (true)
                {

                }
            }
        


            return tempCallNums;


        }

        /// <summary>
        /// Method that will genrate 7 random top level call nums
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> generateRandomAreas() {
            Dictionary<string, string> tempCallNums = new Dictionary<string, string>();
            int count = 0;
            while (count< 7)
            {
                Random rand = new Random();
                Random rand2 = new Random();
                int x = rand.Next(0, identifyAreaList.Count);
                int y = rand2.Next(0, 2);

                if (!tempCallNums.ContainsKey(identifyAreaList.ElementAt(x).Key)&& !tempCallNums.ContainsKey(identifyAreaList.ElementAt(x).Value))
                {
                    if (y == 1)
                    {
                        tempCallNums.Add(identifyAreaList.ElementAt(x).Key, identifyAreaList.ElementAt(x).Value);
                        count++;

                    }
                    else
                    {
                        tempCallNums.Add(identifyAreaList.ElementAt(x).Value, identifyAreaList.ElementAt(x).Key);
                        count++;
                    }
                }
                
            }
            return tempCallNums;
        }

        public int findArea(string arg) {

            if (arg.Length>3)
            {
                for (int i = 0; i < identifyAreaList.Count; i++)
                {
                    if (identifyAreaList.ElementAt(i).Value == arg)
                    {
                        return i;
                    }
                }
            }
            else
            {
                for (int i = 0; i < identifyAreaList.Count; i++)
                {
                    if (identifyAreaList.ElementAt(i).Key == arg)
                    {
                        return i;
                    }
                }
            }
            return -1;

        
        }

        //public Dictionary<string, string> addValidAnswers(List<string> questions, Dictionary<string, string> callnums) {
        //    List<int> tempExisting = new List<int>();

        //    for (int i = 0; i < questions.Count; i++)
        //    {
        //        int x = findArea(questions[i]);
        //        KeyValuePair<string, string> keyValue = identifyAreaList.ElementAt(x);
        //        if (questions[i].Length<3)
        //        {
                   
        //        }
        //        else
        //        {

        //        }
        //    }

        //}
    }
}
