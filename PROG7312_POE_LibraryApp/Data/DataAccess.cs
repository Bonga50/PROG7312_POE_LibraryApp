using PROG7312_POE_LibraryApp.Models;
using System.Drawing;

namespace PROG7312_POE_LibraryApp.Data
{
    public class DataAccess
    {
        //List of Aurthors
        List<string> authors = new List<string>()
        {
            "John Smith", "Jane Austen", "Mark Twain", "Emily Bronte", "Charles Dickens",
            "William Shakespeare", "Virginia Woolf", "Ernest Hemingway", "Scott Fitzgerald", "George Orwell",
            "James Joyce", "Thomas Hardy", "Edgar Poe", "Robert Frost", "Eliot Cummings",
            "Arthur Doyle", "Agatha Christie", "Stephen King", "Rowling Potter", "Lewis Carroll"
        };
        List<string> existingSurnames = new List<string>();

        /// <summary>
        /// Method will generate differnt authors initals 
        /// </summary>
        /// <returns></returns>
        public string getRandomSurnames() {
            bool exits = true;
            string surname = "";
            Random rnd = new Random();

            do
            {
                surname = authors[rnd.Next(0, authors.Count)].Substring(0,3).ToUpper();

                if (!existingSurnames.Contains(surname))
                {
                   exits = false;
                   existingSurnames.Add(surname);
                    
                }


            } while (exits);
          return surname;
        
        }

        /// <summary>
        /// Method that will generate a library call number, returnms an object of the call number
        /// </summary>
        /// <returns></returns>
        public Books generateRandomNums()
        {
            Books rObj = new Books();

            string tempval ="";
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

            rObj.mainNum = tempval + "." + temp2val+" "+surname;
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
        public List<Books> getRandomnums(int size) {
            List<Books> newList = new List<Books>();

            for (int i = 0; i < size; i++)
            {
                newList.Add(generateRandomNums());
            }
            return newList;
        }

        /// <summary>
        /// method that will returnn a sorted list of call numbers
        /// </summary>
        /// <param name="unSorted"></param>
        /// <returns></returns>
        public List<Books> getSortednums(List<Books> unSorted)
        {
            List<Books> newList = new List<Books>();

            for (int i = 0; i < unSorted.Count; i++)
            {
                newList.Add(generateRandomNums());
            }
            return newList;
        }
    }
}
