using com.zhong.chinese_calendar.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace com.zhong.chinese_calendar.Tests
{
    [TestClass()]
    public class JsonHelperTests
    {
        private static string _jsonFilePath = Path.Combine(Path.GetTempPath(), "special_dates.json");

        private YearSpecialDate _year2024SpecialDate = new YearSpecialDate(2024,
            new List<SpecialDate>() { new SpecialDate {
                Date = new DateTime(2024,1,1),
                Holiday = true,
                Name = "元旦",
                Rest = 1
            } });

        private YearSpecialDate _year2022SpecialDate = new YearSpecialDate(2024,
            new List<SpecialDate>() { new SpecialDate {
                Date = new DateTime(2022,1,1),
                Holiday = true,
                Name = "元旦",
                Rest = 1
            },
                new SpecialDate {
                Date = new DateTime(2022,10,1),
                Holiday = true,
                Name = "国庆节",
                Rest = 1
            }

            }); 
        
        [TestMethod()]
        public void SaveToJsonFile_LoadJsonFile_Test()
        {
            JsonHelper<YearSpecialDate>.SaveToFile(_jsonFilePath, new List<YearSpecialDate>() {
                _year2024SpecialDate
            });

            var yearsResults = JsonHelper<YearSpecialDate>.LoadJson(_jsonFilePath);
            Assert.IsTrue(yearsResults.Count() == 1);
            Assert.IsTrue(yearsResults.First().SpecialDates.Count() == 1);
            Assert.IsTrue(yearsResults.First().SpecialDates.First().Name == "元旦");

            JsonHelper<YearSpecialDate>.SaveToFile(_jsonFilePath, new List<YearSpecialDate>() {
                _year2024SpecialDate,
                _year2022SpecialDate
            });

            yearsResults = JsonHelper<YearSpecialDate>.LoadJson(_jsonFilePath);
            Assert.IsTrue(yearsResults.Count() == 2);
            Assert.IsTrue(yearsResults.Last().SpecialDates.Count() == 2);
            Assert.IsTrue(yearsResults.Last().SpecialDates.Last().Name == "国庆节"); 
        } 
    }
}