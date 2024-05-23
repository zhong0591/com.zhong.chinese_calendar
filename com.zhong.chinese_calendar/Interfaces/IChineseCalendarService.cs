using com.zhong.chinese_calendar.Models;

namespace com.zhong.chinese_calendar.Interfaces
{
    public interface IChineseCalendarService<T>
    {
        Task<T> GetYearSpecialDatesFromRemoteAsync(int year);
        Task<T> GetYearSpecialDatesAsync(int year, string webRootPath);
        Task<bool> CheckDateIsHolidayAsync(DateTime date, string webRootPath);
    }
}
