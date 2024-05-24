using com.zhong.chinese_calendar.Helpers;
using com.zhong.chinese_calendar.Interfaces;
using com.zhong.chinese_calendar.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;

namespace com.zhong.chinese_calendar.Services
{
    public class ChineseCalendarService : IChineseCalendarService<YearSpecialDate>
    {
        public static readonly string SPECIAL_DATES_JSON_FILE_NAME = "special_dates.json";
        private static readonly string HOLIDAY_API_URL = "http://timor.tech/api/holiday/year/";
        private readonly ILogger<ChineseCalendarService> _logger;

        public ChineseCalendarService(ILogger<ChineseCalendarService> logger)
        {
            _logger = logger;
        }

        public async Task<YearSpecialDate> GetYearSpecialDatesFromRemoteAsync(int year)
        {
            var url = HOLIDAY_API_URL + year;
            var returnMsg = await GetHTMLReturnMsg(url);
            if (returnMsg.Success && !string.IsNullOrEmpty(returnMsg.Result))
            {
                var specialDates = GetSpecialDatesByHtml(returnMsg.Result);
                return new YearSpecialDate(year, specialDates);
            }
            else
            {
                return new YearSpecialDate(year, new List<SpecialDate>());
            }
        }

        public async Task<YearSpecialDate> GetYearSpecialDatesAsync(int year, string webRootPath)
        {
            //Special dates already saved in json file
            var jsonFileFullPath = Path.Combine(webRootPath, SPECIAL_DATES_JSON_FILE_NAME);
            var yearSpeicalDates = JsonHelper<YearSpecialDate>.LoadJson(jsonFileFullPath);
            var yearSpecialDate = yearSpeicalDates.FirstOrDefault(x => x.Year == year);
            if (yearSpecialDate != null)
            {
                return yearSpecialDate;
            }

            //Special dates need reload from api, after loaded, save data to json file.
            var newYearSpecialDate = await GetYearSpecialDatesFromRemoteAsync(year);
            if (newYearSpecialDate.SpecialDates.Any())
            {
                yearSpeicalDates.Add(newYearSpecialDate);
                JsonHelper<YearSpecialDate>.SaveToFile(jsonFileFullPath, yearSpeicalDates);
            }
            return newYearSpecialDate;
        }

        public async Task<bool> CheckDateIsHolidayAsync(DateTime date, string webRootPath)
        {
            var yearSpecialDate = await GetYearSpecialDatesAsync(date.Year, webRootPath);
            return CheckHoliday(date, yearSpecialDate.SpecialDates);
        }

        private static bool CheckHoliday(DateTime date, IEnumerable<SpecialDate> specialDates)
        {
            var specialHoliday = specialDates.FirstOrDefault(x => x.Date == date);
            if (specialHoliday != null) { return specialHoliday.Holiday; }
            return date.DayOfWeek == DayOfWeek.Saturday || date.DayOfWeek == DayOfWeek.Sunday;
        }

        private async Task<ReturnMsg<string>> GetHTMLReturnMsg(string url)
        {
            var retMsg = new ReturnMsg<string>();
            HttpClient httpClient = new HttpClient();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(url);
                var successResponse = response.EnsureSuccessStatusCode();
                retMsg.ReasonPhrase = successResponse.ReasonPhrase;
                retMsg.Result = await response.Content.ReadAsStringAsync();
                retMsg.StatusCode = successResponse.StatusCode;
            }
            catch (HttpRequestException e)
            {
                SerilogHelper.LogError(_logger, e.Message);
                retMsg.ReasonPhrase = e.Message;
                retMsg.StatusCode = System.Net.HttpStatusCode.NotFound;
            }
            return retMsg;
        }

        private IEnumerable<SpecialDate> GetSpecialDatesByHtml(string strHtml)
        {
            var specialDates = new List<SpecialDate>();
            JObject jsonObject = JObject.Parse(strHtml);
            var FestivalHolidays = jsonObject["holiday"]?.Children<JProperty>();
            if (FestivalHolidays != null)
            {
                foreach (var holiday in FestivalHolidays)
                {
                    var holidayDetails = holiday.Children();
                    foreach (var holidayDetail in holidayDetails)
                    {
                        Debug.WriteLine(holidayDetail.ToString(Formatting.Indented));
                        var SpecialDate = JsonConvert.DeserializeObject<SpecialDate>(holidayDetail.ToString());
                        if (SpecialDate != null)
                        {
                            specialDates.Add(SpecialDate);
                        }
                    }
                }
            }
            return specialDates;
        }
    }
}
