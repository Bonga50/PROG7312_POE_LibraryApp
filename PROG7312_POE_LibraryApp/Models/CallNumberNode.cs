namespace PROG7312_POE_LibraryApp.Models
{
    public class CallNumberNode
    {
        public string callNumber { get; set; }
        public string description { get; set; }
        public CallNumberNode callNumberParent { get; set; }
        public  List<CallNumberNode> callNumberChildern { get; set; }

        public CallNumberNode() { }

        public CallNumberNode(string callNumber, string description, CallNumberNode callNumberParent)
        {
            this.callNumber = callNumber;
            this.description = description;
            this.callNumberParent = callNumberParent;
            callNumberChildern = new List<CallNumberNode>();
        }

        public void addChildCallnumber(string value, string desc)
        {
            this.callNumberChildern.Add(new CallNumberNode(value, desc, this));
        }

        public override string? ToString()
        {
            return $"{callNumber} , {description}";
        }
    }
}
