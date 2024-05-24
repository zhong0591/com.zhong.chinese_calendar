namespace com.zhong.chinese_calendar.Models
{
    public class CustomizeException : Exception
    {
        public CustomizeException()
        {

        }

        public CustomizeException(string message)
        {
            FriendlyMessage = message;
        }

        public string? FriendlyMessage { get; set; }
    }
}
