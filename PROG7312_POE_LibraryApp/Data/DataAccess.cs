using Microsoft.AspNetCore.Routing.Template;
using PROG7312_POE_LibraryApp.Models;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
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
            List<string> tempNumsAnswers = new List<string>();

            List<int> tempExisting = new List<int>();

          
            tempNums = generateRandomAreas();
            tempNumsAnswers = generateRandomAreasCorrosponding(tempNums);

            for (int i = 0; i < tempNumsAnswers.Count; i++)
            {
                tempCallNums.Add(tempNums[i], tempNumsAnswers[i]);
            }
        

            return tempCallNums;


        }

        /// <summary>
        /// Method that will genrate 7 random top level call nums
        /// </summary>
        /// <returns></returns>
        public List<string> generateRandomAreas() {
            List<string> tempCallNums = new List<string>();
            int count = 0;
            while (count< 7)
            {
                Random rand = new Random();
                Random rand2 = new Random();
                int x = rand.Next(0, identifyAreaList.Count);
                int y = rand2.Next(0, 2);
                //add a key
                if (y==1)
                {
                    if (!tempCallNums.Contains(identifyAreaList.ElementAt(x).Key) &&
                        !tempCallNums.Contains(findCorosspondingAnswer(identifyAreaList.ElementAt(x).Key)))
                    {
                        tempCallNums.Add(identifyAreaList.ElementAt(x).Key);
                        count++;
                    }
                }
                //add a value
                else
                {
                    if (!tempCallNums.Contains(identifyAreaList.ElementAt(x).Value) &&
                        !tempCallNums.Contains(findCorosspondingAnswer(identifyAreaList.ElementAt(x).Value)))
                    {
                        tempCallNums.Add(identifyAreaList.ElementAt(x).Value);
                        count++;
                    }

                }
                
            }
            return tempCallNums;
        }

        public List<string> generateRandomAreasCorrosponding(List<string> callnums) {
            List<string> answers = new List<string>();
            for (int i = 0; i < callnums.Count; i++)
            {
                answers.Add(findCorosspondingAnswer(callnums[i]));
            }
            answers = shuffleList(answers);
            return answers;

        }
        public string findCorosspondingAnswer(string arg) {

            if (arg.Length<=3)
            {
                for (int i = 0; i < identifyAreaList.Count; i++)
                {
                    if (identifyAreaList.ElementAt(i).Key==arg)
                    {
                        return identifyAreaList.ElementAt(i).Value;
                    }
                }
            }
            else
            {
                for (int i = 0; i < identifyAreaList.Count; i++)
                {

                    if (identifyAreaList.ElementAt(i).Value == arg)
                    {
                        return identifyAreaList.ElementAt(i).Key;
                    }

                }
            }
            return "";
        }
      
        /// <summary>
        /// method to shuffle the list of pottential answers
        /// </summary>
        /// <param name="answers"></param>
        /// <returns></returns>
        public List<string> shuffleList(List<string> answers) {

            Random random = new Random();
            List<string> newShuffledList = new List<string>();
            int listcCount = answers.Count;
            for (int i = 0; i < listcCount; i++)
            {
                var randomElementInList = random.Next(0, answers.Count);
                newShuffledList.Add(answers[randomElementInList]);
                answers.Remove(answers[randomElementInList]);
            }
            return newShuffledList;
        }

        public bool checkIdentifiedAreas(Dictionary<string,string> userAnswers)
        {
            if (userAnswers.Count ==0)
            {
                return false;
            }
            bool correct = true;
            for (int i = 0; i < userAnswers.Count; i++)
            {
                string expectedAnswer = findCorosspondingAnswer(userAnswers.ElementAt(i).Key);
                if (userAnswers.ElementAt(i).Value!=expectedAnswer)
                {
                    correct = false;
                    break;  
                }

            }

            return correct;
        }

    }
}
