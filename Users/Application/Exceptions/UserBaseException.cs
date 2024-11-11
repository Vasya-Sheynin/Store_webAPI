namespace Users.Application.Exceptions
{
    public class UserBaseException : Exception
    {
        public string Type { get; set; }
        public string Detail { get; set; }
        public string Title { get; set; }
        public string Instance { get; set; }
    }
}
