using com.zhong.chinese_calendar.Helpers;
using com.zhong.chinese_calendar.Interfaces;
using com.zhong.chinese_calendar.Models;
using Microsoft.AspNetCore.Mvc;

namespace com.zhong.chinese_calendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChineseCalendarController : ControllerBase
    {
        private readonly IChineseCalendarService<YearSpecialDate> _chineseCalendarService;
        private readonly ILogger<ChineseCalendarController> _logger;
        private readonly IWebHostEnvironment _env;

        public ChineseCalendarController(IChineseCalendarService<YearSpecialDate> chineseCalendarService,
            IWebHostEnvironment env, ILogger<ChineseCalendarController> logger)
        {
            _chineseCalendarService = chineseCalendarService;
            _logger = logger;
            _env = env;
        }

        /// <summary>
        /// Get the indicated year's all special dates.
        /// </summary>
        /// <param name="year">year</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetYearSpecialDatesFromRemote/{year}")]
        public async Task<YearSpecialDate> GetYearSpecialDatesFromRemote(int year)
        {
            try
            {
                return await _chineseCalendarService.GetYearSpecialDatesFromRemoteAsync(year);
            }
            catch (Exception ex)
            {
                SerilogHelper.LogError(_logger, ex.Message);
                return new YearSpecialDate(year, new List<SpecialDate>());
            }
        }


        /// <summary>
        /// Get the indicated year's all special dates.
        /// </summary>
        /// <param name="year">year</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetYearSpecialDates/{year}")]
        public async Task<YearSpecialDate?> GetYearSpecialDates(int year)
        {
            var webRootPath = _env.ContentRootPath;
            try
            {
                return await _chineseCalendarService.GetYearSpecialDatesAsync(year, webRootPath);
            }
            catch (Exception ex)
            {
                SerilogHelper.LogError(_logger, ex.Message);
                return new YearSpecialDate(year, new List<SpecialDate>());
            }
        }

        /// <summary>
        /// Check a date is holiday or work day. 
        /// </summary>
        /// <param name="date">Date format should be 'yyyy-MM-dd'</param>
        /// <returns></returns>
        [HttpGet]
        [Route("CheckDateIsHoliday/{date}")]
        public async Task<ReturnMsg<bool>> CheckDateIsHoliday(DateTime date)
        {
            _logger.LogInformation("Work Date ===========> " + date);
            var msg = new ReturnMsg<bool>();
            if (ModelState.IsValid)
            {
                try
                {
                    msg.Result = await _chineseCalendarService.CheckDateIsHolidayAsync(date, _env.ContentRootPath);
                } 
                catch (Exception ex)
                {
                    SerilogHelper.LogError(_logger, ex.Message);
                }
            }
            else
            { 
                SerilogHelper.LogError(_logger, ErrorMessage.ERR_MODEL_STATE + ModelState.Select(x => "<<<" + x.Value + ">>>"));
            }
            return msg;
        }
    }
}
