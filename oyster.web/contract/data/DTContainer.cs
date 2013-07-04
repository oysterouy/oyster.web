using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;

namespace oyster.web
{
    [Serializable]
    public class DTContainer
    {
        public HttpContext Context { get; set; }
        public RequestInfo Parameters { get; set; }
        public Hashtable Data { get; set; }
        public StringBuilder RanderResult { get; set; }
        public static DTContainer Current { get { return null; } }
    }
}
