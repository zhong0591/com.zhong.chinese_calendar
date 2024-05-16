using System.Net;

namespace com.zhong.chinese_calendar.Models
{
    public class ReturnMsg<T>
    {
        public ReturnMsg()
        { 
        }
        public ReturnMsg(  HttpStatusCode statusCode, string? reasonPhrase)
        {
            StatusCode = statusCode;
            ReasonPhrase = reasonPhrase;
        }

        public bool Success { get { return string.IsNullOrEmpty(ReasonPhrase); } }
        public HttpStatusCode StatusCode { get; set; } = HttpStatusCode.OK;
        public string? ReasonPhrase { get; set; } 
        public T? Result { get; set; }
    }
}
