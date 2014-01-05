using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web
{
    [Serializable]
    public class ResponseHead
    {
        public ResponseHead()
        {
            StatusCode = 200;
            Cookies = new HttpCookieCollection();
        }
        public int StatusCode { get; set; }

        public string RedirectLocation { get; set; }

        public HttpCookieCollection Cookies { get; set; }
    }
}
