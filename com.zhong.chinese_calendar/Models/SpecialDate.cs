namespace com.zhong.chinese_calendar.Models
{
    public class SpecialDate
    {
        public bool Holiday { get; set; }
        public string Name { get; set; } = string.Empty;
        public int Wage { get; set; }
        public DateTime Date { get; set; }
        public int Rest { get; set; }
    }
}
