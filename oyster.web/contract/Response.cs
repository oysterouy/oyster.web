using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web
{
    [Serializable]
    public class Response
    {
        public Response()
        {
            Body = new StringBuilder();
            Model = new TimDynamicModel();
            Cookies = new List<HttpCookie>();
            Headers = new List<KeyValuePair<string, string>>();
        }
        public object[] Paramters { get; set; }
        public dynamic Model { get; set; }
        public StringBuilder Body { get; set; }

        public int StatusCode { get; set; }
        public string RedirectLocation { get; set; }

        public List<HttpCookie> Cookies { get; set; }

        public List<KeyValuePair<string, string>> Headers { get; set; }
    }
}
