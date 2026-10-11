namespace COLLABHUB
{
    public class TaskItem
    {
        public string Title;
        public string Priority;
        public bool IsDone;

        public string ToText()
        {
            string status = IsDone ? "Done" : "Pending";
            return Title + "  |  " + Priority + "  |  " + status;
        }
    }
}