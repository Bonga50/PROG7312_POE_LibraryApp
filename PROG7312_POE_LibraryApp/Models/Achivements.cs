namespace PROG7312_POE_LibraryApp.Models
{
    public class Achivements
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string Criteria { get; set; }
        public bool IsSecret { get; set; }
        public bool IsUnlocked { get; set; }

        public Achivements()
        {

        }

        public Achivements(int iD, string name, string description, string icon, string criteria, bool isSecret, bool isUnlocked)
        {
            ID = iD;
            Name = name;
            Description = description;
            Icon = icon;
            Criteria = criteria;
            IsSecret = isSecret;
            IsUnlocked = isUnlocked;
        }
    }
}
