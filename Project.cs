namespace COLLABHUB
{
    public class Project
    {
        public string Title;
        public string Description;
        public string Owner;

        public string ToText()
        {
            return Title + "  |  " + Description + "  |  By: " + Owner;
        }
    }
}