using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web
{
    public interface ISetting
    {
        ITemplate Route(HttpContext context);
        bool Filter(FilterOn fon, HttpContext context, ITemplate it, StringBuilder str);
    }
    public interface ITemplate
    {
        ResponseDto Entrance(RequestDto request);
    }

    public enum FilterOn
    {
        BeforeRoute = 1,
        AfterRoute = 2,
        BeforeExport = 3,
    }

    public class RequestDto
    {
        public HttpRequest Request { get; set; }
    }

    public class ResponseDto
    {
        public ResponseDto()
        {
            Headers = new Dictionary<string, string>();
            Cookies = new List<HttpCookie>();
        }
        public Dictionary<string, string> Headers { get; set; }
        public List<HttpCookie> Cookies { get; set; }
        public StringBuilder Body { get; set; }

        public string RedirectLocation { get; set; }

        public string Status { get; set; }

        public int StatusCode { get; set; }
    }
}
