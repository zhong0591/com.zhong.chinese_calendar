using com.zhong.chinese_calendar.Models;
using com.zhong.chinese_calendar.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Net;

namespace com.zhong.chinese_calendar.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChineseCalendarController : ControllerBase
    {
        private readonly ILogger<ChineseCalendarController> _logger;
        private readonly IWebHostEnvironment _env;

        public ChineseCalendarController(IWebHostEnvironment env, ILogger<ChineseCalendarController> logger)
        {
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
            ChineseCalendarService service = new ChineseCalendarService();
            return await service.GetYearSpecialDatesFromRemoteAsync(year);
            //return await ChineseCalendarHelper.GetYearSpecialDatesFromRemoteAsync(year);
        }


        /// <summary>
        /// Get the indicated year's all special dates.
        /// </summary>
        /// <param name="year">year</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetYearSpecialDates/{year}")]
        public async Task<YearSpecialDate> GetYearSpecialDates(int year)
        {
            var webRootPath = _env.ContentRootPath;
            ChineseCalendarService service = new ChineseCalendarService();
            return await service.GetYearSpecialDatesAsync(year, webRootPath);
            //return await ChineseCalendarHelper.GetSpecialDatesByYearAsync(year, webRootPath);
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
            var msg = new ReturnMsg<bool>();
            if (ModelState.IsValid)
            {
                try
                {
                    msg.Result = await new ChineseCalendarService().CheckDateIsHolidayAsync(date, _env.ContentRootPath);
                    //msg.Result = await ChineseCalendarHelper.CheckDateIsHolidayAsync(date, _env.ContentRootPath);
                }
                catch (HttpRequestException hre)
                {
                    msg.StatusCode = HttpStatusCode.BadRequest;
                    msg.ReasonPhrase = hre.Message;
                }
                catch (CustomizeException cex)
                {
                    Debug.WriteLine(cex.Message);
                    msg.ReasonPhrase = cex.FriendlyMessage;
                }
            }
            else
            {
                msg.ReasonPhrase = ErrorMessage.ERR_MODEL_STATE;
            }
            return msg;
        }
    }
}
