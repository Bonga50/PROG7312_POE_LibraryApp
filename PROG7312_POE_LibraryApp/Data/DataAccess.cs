using PROG7312_POE_LibraryApp.Models;
using System.Drawing;

namespace PROG7312_POE_LibraryApp.Data
{
    public class DataAccess
    {

        public Replacingbooks generateRandomNums()
        {
            Replacingbooks rObj = new Replacingbooks();

            string tempval ="";
            string temp2val = "";
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

            rObj.mainNum = tempval+"."+temp2val;

            return rObj;
        }

        public int generateNum()
        {
            Random rand = new Random();
            
            return rand.Next(0, 9);
        }

        public List<Replacingbooks> getRandomnums(int size) {
            List<Replacingbooks> newList = new List<Replacingbooks>();

            for (int i = 0; i < size; i++)
            {
                newList.Add(generateRandomNums());
            }
            return newList;
        }

        public List<Replacingbooks> getSortednums(List<Replacingbooks> unSorted)
        {
            List<Replacingbooks> newList = new List<Replacingbooks>();

            for (int i = 0; i < unSorted.Count; i++)
            {
                newList.Add(generateRandomNums());
            }
            return newList;
        }
    }
}
