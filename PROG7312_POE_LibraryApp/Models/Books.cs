namespace PROG7312_POE_LibraryApp.Models
{
    public class Books
    {
        public string mainNum { get; set; }
        public string groupNum { get; set; }
        public string subGroupNum { get; set; }
        public string authirInitials { get; set; }

        public Books()
        {
                
        }

        public Books(string mainNum, string groupNum, string subGroupNum, string authirInitials)
        {
            this.mainNum = mainNum;
            this.groupNum = groupNum;
            this.subGroupNum = subGroupNum;
            this.authirInitials = authirInitials;
        }
    }
}
