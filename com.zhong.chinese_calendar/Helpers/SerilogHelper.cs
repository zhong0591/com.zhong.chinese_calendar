namespace com.zhong.chinese_calendar.Helpers
{
    public class SerilogHelper
    {
        public static void LogError(ILogger logger, string message)
        {
            logger.LogError("{@Class}：{@Message}", logger.GetType().FullName, message);
        }

        public static void LogInformation(ILogger logger, string message)
        {
            logger.LogInformation("{@Class}：{@Message}", logger.GetType().FullName, message);
        }

        public static void LogWarning(ILogger logger, string message)
        {
            logger.LogWarning("{@Class}：{@Message}", logger.GetType().FullName, message);
        }
    }
}
