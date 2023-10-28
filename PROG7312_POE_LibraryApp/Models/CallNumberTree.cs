namespace PROG7312_POE_LibraryApp.Models
{
    public class CallNumberTree
    {
        public CallNumberNode root { get; set; }

        public CallNumberTree(CallNumberNode root)
        {
            this.root = root;
        }
    }
}
