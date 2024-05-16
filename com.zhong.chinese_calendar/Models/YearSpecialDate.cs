namespace com.zhong.chinese_calendar.Models
{
    public class YearSpecialDate
    {
        public YearSpecialDate(int year, IEnumerable<SpecialDate> specialDates)
        {
            Year = year;
            SpecialDates = specialDates.ToList();
        }
    
        public int Year { get; set; }
        public List<SpecialDate> SpecialDates { get; set; } = new List<SpecialDate>();
    }
}
