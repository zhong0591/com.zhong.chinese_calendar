using com.zhong.chinese_calendar.Helpers;
using com.zhong.chinese_calendar.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.zhong.chinese_calendar.Tests
{
    [TestClass()]
    public class ChineseCalendarHelperTests
    {
        private static string _jsonFilePath = Path.Combine(Path.GetTempPath(), "special_dates.json");

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        { 
        }

        [TestMethod()]
        public async Task GetSpecialDatesByYearTest()
        {
            var result = await ChineseCalendarHelper.GetYearSpecialDatesFromRemoteAsync(2024);

            Assert.IsTrue(result.Year == 2024);
            Assert.IsTrue(result.SpecialDates.Count() == 38);
            Assert.IsTrue(result.SpecialDates.First().Name == "元旦");
            Assert.IsTrue(result.SpecialDates.Last().Name == "国庆节后补班");
        }



        [TestMethod()]
        public async Task CheckDateIsHolidayTest()
        {
            var datetime = new DateTime(2024, 5, 5);
            var isHoliday = await ChineseCalendarHelper.CheckDateIsHolidayAsync(datetime, _jsonFilePath);
            Assert.IsTrue(isHoliday);

            datetime = new DateTime(2024, 5, 11);
            isHoliday = await ChineseCalendarHelper.CheckDateIsHolidayAsync(datetime, _jsonFilePath);
            Assert.IsFalse(isHoliday);

            datetime = new DateTime(2024, 4, 7);
            isHoliday = await ChineseCalendarHelper.CheckDateIsHolidayAsync(datetime, _jsonFilePath);
            Assert.IsFalse(isHoliday);
        }

        [TestMethod()]
        public async Task GetSpecialDatesByYearAsyncTest()
        {
            var envPath = Path.GetTempPath();
            var result = await ChineseCalendarHelper.GetSpecialDatesByYearAsync(2024, envPath); 
            Assert.IsTrue(result.Year == 2024);
            Assert.IsTrue(result.SpecialDates.Count() == 38);
            Assert.IsTrue(result.SpecialDates.First().Name == "元旦");
            Assert.IsTrue(result.SpecialDates.Last().Name == "国庆节后补班"); 
        }

        [TestMethod()] 
        public async Task SaveYearSpecialDatesToJsonFile_LoadJson_Test()
        {
            var envPath = Path.GetTempPath();
            await ChineseCalendarHelper.GetSpecialDatesByYearAsync(2024, envPath);
            var yearResults =  JsonHelper<YearSpecialDate>.LoadJson(_jsonFilePath); 
            Assert.IsTrue(yearResults.Count() == 1);
            Assert.IsTrue(yearResults.First().SpecialDates.Count() == 38 );

            await ChineseCalendarHelper.GetSpecialDatesByYearAsync(2022, envPath);
            yearResults = JsonHelper<YearSpecialDate>.LoadJson(_jsonFilePath);
            Assert.IsTrue(yearResults.Count() == 2);

            await ChineseCalendarHelper.GetSpecialDatesByYearAsync(System.DateTime.Now.Year + 1, envPath);
            yearResults = JsonHelper<YearSpecialDate>.LoadJson(_jsonFilePath);
            Assert.IsTrue(yearResults.Count() == 2);
        }

        [TestCleanup()]
        public void ClearUpTestJsonFile()
        { 
            if (File.Exists(_jsonFilePath))
            {
                File.Delete(_jsonFilePath);
            }
        }

        [TestMethod()]
        public void GetSpecialDatesByYearAsyncTest1()
        {
            Assert.Fail();
        }
    } 
}