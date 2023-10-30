using Microsoft.AspNetCore.Routing.Template;
using PROG7312_POE_LibraryApp.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Xml.Linq;

namespace PROG7312_POE_LibraryApp.Data
{
    public class DataAccess
    {

        private static DataAccess _instance = null;
        private static readonly object padlock = new object();
        public List<Books> randomNums = new List<Books>();
        public Dictionary<string, string> areaCallNums = new Dictionary<string, string>();
        public List<identifyArea> identifyAreaBooks = new List<identifyArea>();
        public List<CallNumberNode> findingCallNumberList = new List<CallNumberNode>();
        public CallNumberNode selectedRandomCallNumNode = new CallNumberNode();
        public int findingCallnumberLevel { get; set; } = 1;
        public int mySwitch { get; set; }


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
        /// Method to control alternation between call nums and descriptions
        /// </summary>
        /// <returns></returns>
        public int getSwitch() {
            if (mySwitch == 1)
            {
                mySwitch = 2;
                return mySwitch;
            }
            else
            {
                mySwitch = 1;
                return mySwitch;
            }

        }

        /// <summary>
        /// Method that will genrate 7 random top level call nums
        /// </summary>
        /// <returns></returns>
        public List<string> generateRandomAreas() {
            List<string> tempCallNums = new List<string>();
            int count = 0;
            int y = getSwitch();
            while (count < 7)
            {
                Random rand = new Random();
                Random rand2 = new Random();
                int x = rand.Next(0, identifyAreaList.Count);

                //add a key
                if (y == 1)
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
        /// <summary>
        /// Method that wil generate the pottential answers to match the generated list and suffle them.
        /// </summary>
        /// <param name="callnums"></param>
        /// <returns></returns>
        public List<string> generateRandomAreasCorrosponding(List<string> callnums) {
            List<string> answers = new List<string>();
            for (int i = 0; i < callnums.Count; i++)
            {
                answers.Add(findCorosspondingAnswer(callnums[i]));
            }
            answers = shuffleList(answers);
            return answers;

        }

        /// <summary>
        /// Method that will get the value if the key is parsed and will get the key if the value is parsed.
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        public string findCorosspondingAnswer(string arg) {

            if (arg.Length <= 3)
            {
                for (int i = 0; i < identifyAreaList.Count; i++)
                {
                    if (identifyAreaList.ElementAt(i).Key == arg)
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
        /// <summary>
        /// Method that will compare users answers to the correct answers.
        /// </summary>
        /// <param name="userAnswers"></param>
        /// <returns></returns>
        public bool checkIdentifiedAreas(Dictionary<string, string> userAnswers)
        {
            if (userAnswers.Count == 0)
            {
                return false;
            }
            bool correct = true;
            for (int i = 0; i < userAnswers.Count; i++)
            {
                string expectedAnswer = findCorosspondingAnswer(userAnswers.ElementAt(i).Key);
                if (userAnswers.ElementAt(i).Value != expectedAnswer)
                {
                    correct = false;
                    break;
                }

            }

            return correct;
        }

        public List<CallNumberNode> getCallNumbersFromTextFile() {
            string path = getPath();
            CallNumberTree Tree = BuildTree(path);
            CallNumberNode randomCallNumberNode = getRandomCallNumberNode(Tree.root);
            selectedRandomCallNumNode = randomCallNumberNode;
            List<CallNumberNode> numberNodes = getRandomPotentialAnswers(Tree.root, randomCallNumberNode,findingCallnumberLevel);
            findingCallNumberList = numberNodes;
            return numberNodes;
            
        }
        /// <summary>
        /// Method to get the path to the path for the text file
        /// </summary>
        /// <returns>the path of the text file</returns>
        public string getPath() {
            string path = "CallNumbers.txt";
            string pathToFile = AppDomain.CurrentDomain.BaseDirectory + $"\\{path}";
            string filePath = Path.GetFullPath(pathToFile).Replace(@"\bin\Debug\net6.0", @"\Data");
            return filePath;
        }
        /// <summary>
        /// Method that will poulate the list and all levels
        /// </summary>
        /// <param name="path"></param>
        /// <returns>A tree of the call numbers</returns>
        public CallNumberTree BuildTree(string path)
        {
            var nodes = new Dictionary<string, CallNumberNode>();
            CallNumberNode root = new CallNumberNode() { callNumber = "0", description = "", callNumberParent = null, callNumberChildern = new List<CallNumberNode>() };

            using (StreamReader sr = new StreamReader(path))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    int currentLevel1Index = 0;
                    int currentLevel2Index = 0;
                    string[] parts = line.Split(new[] { ',' }, 2);
                    if (line.Contains(" - "))
                    {

                        parts = line.Split(new[] { '-' }, 2);
                        root.addChildCallnumber(parts[0], parts[1]);
                    }
                    else
                    {
                        parts = line.Split(new[] { ',' }, 2);
                        currentLevel1Index = Int32.Parse(parts[0].Substring(0, 1));
                        int callNumber = int.Parse(parts[0]);
                        if (callNumber % 10 == 0)
                        {
                            root.callNumberChildern[currentLevel1Index].addChildCallnumber(parts[0], parts[1]);
                            currentLevel2Index = 0;
                        }
                        else
                        {
                            currentLevel2Index = Int32.Parse(parts[0].Substring(1, 1));
                            root.callNumberChildern[currentLevel1Index].callNumberChildern[currentLevel2Index].addChildCallnumber(parts[0], parts[1]);
                        }


                    }


                }
            }
            CallNumberTree callNumbers = new CallNumberTree(root);
            return callNumbers;

        }
        /// <summary>
        /// Method that will get a random call number node from the tree root.
        /// </summary>
        /// <param name="root"></param>
        /// <returns>A random call number node</returns>
        public CallNumberNode getRandomCallNumberNode(CallNumberNode root){
            Random random = new Random();
            bool badcallnumber = true;
            CallNumberNode randomCallNumber = new CallNumberNode();
            do
            {
                int level1Random = random.Next(0, root.callNumberChildern.Count);
                int level2Random = random.Next(0, root.callNumberChildern[level1Random].callNumberChildern.Count);
                int level3Random = random.Next(1, root.callNumberChildern[level1Random].callNumberChildern[level2Random].callNumberChildern.Count);
                randomCallNumber = root.callNumberChildern[level1Random].callNumberChildern[level2Random].callNumberChildern[level3Random];
                if (!randomCallNumber.description.Contains("[Unassigned]") && !randomCallNumber.description.Contains("(Optional number)"))
                {
                    badcallnumber = false;
                }
            } while (badcallnumber);
        
            return randomCallNumber;
        }

        public List<CallNumberNode> getRandomPotentialAnswers(CallNumberNode root, CallNumberNode randomNode,int level) {
            List<CallNumberNode> potentialAnswers = new List<CallNumberNode>();
            if (level == 1)
            {
                potentialAnswers.Add(randomNode.callNumberParent.callNumberParent);

                while (potentialAnswers.Count!=4)
                {
                    Random random = new Random();

                        int level1Random = random.Next(0, root.callNumberChildern.Count);
                        if (!potentialAnswers.Contains(root.callNumberChildern[level1Random])&&
                         !root.callNumberChildern[level1Random].description.Contains("[Unassigned]")&&
                         !root.callNumberChildern[level1Random].description.Contains("(Optional number)")
                        )
                        {
                            potentialAnswers.Add(root.callNumberChildern[level1Random]);
                        }
                    
                }
            }
            else if (level == 2)
            {
                potentialAnswers.Add(randomNode.callNumberParent);
                while (potentialAnswers.Count != 4)
                {
                    Random random = new Random();
                    int levelofRandom = int.Parse(randomNode.callNumber.Substring(0, 1));
                    int level2Random = random.Next(0, root.callNumberChildern[levelofRandom].callNumberChildern.Count);
                    if (!potentialAnswers.Contains(root.callNumberChildern[levelofRandom].callNumberChildern[level2Random])&&
                        !root.callNumberChildern[levelofRandom].callNumberChildern[level2Random].description.Contains("[Unassigned]")&&
                        !root.callNumberChildern[levelofRandom].callNumberChildern[level2Random].description.Contains("(Optional number)"))
                    {
                        potentialAnswers.Add(root.callNumberChildern[levelofRandom].callNumberChildern[level2Random]);
                    }

                }
            }
            else
            {
                potentialAnswers.Add(randomNode);
                while (potentialAnswers.Count != 4)
                {
                    Random random = new Random();

                    int level3Random = random.Next(0, randomNode.callNumberParent.callNumberChildern.Count);
                    if (!potentialAnswers.Contains(randomNode.callNumberParent.callNumberChildern[level3Random]) &&
                        !randomNode.callNumberParent.callNumberChildern[level3Random].description.Contains("[Unassigned]")&&
                        !randomNode.callNumberParent.callNumberChildern[level3Random].description.Contains("(Optional number)"))
                    {
                        potentialAnswers.Add(randomNode.callNumberParent.callNumberChildern[level3Random]);
                    }
                }

            }
            potentialAnswers = potentialAnswers.OrderBy(x => x.callNumber).ToList();
            return potentialAnswers;
        }

        public bool checkSelectedFoundNumber(string numbers)
        {
            bool result = false;
            if (findingCallnumberLevel == 1){
                if (selectedRandomCallNumNode.callNumberParent.callNumberParent.callNumber == numbers)
                {
                    findingCallnumberLevel = 2;
                    result = true;
                }
            }
            else if (findingCallnumberLevel == 2){
                if (selectedRandomCallNumNode.callNumberParent.callNumberParent.callNumber == numbers)
                {
                    findingCallnumberLevel = 3;
                    result = true;
                }
            }
            else{

            }

            return result;
        }

    }
}
