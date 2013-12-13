using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace oyster.web
{
    public enum HttpMethod
    {
        GET,
        POST,
        PUT,
        DEL
    }
    public enum CacheToWhere
    {
        Brose = 1,
        Local = 1 << 1,
        Third = 1 << 2
    }
    public class RequestBody
    {
        public CacheToWhere CacheToWhere { get; set; }
        public Func<object[], string> CacheKeySelect { get; set; }
        public object[] Paramters { get; set; }
    }
}
