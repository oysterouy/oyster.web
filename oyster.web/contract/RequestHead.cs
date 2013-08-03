using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace oyster.web
{
    public enum CacheToWhere
    {
        Brose = 1,
        Local = 1 << 1,
        Third = 1 << 2
    }
    [Serializable]
    public class RequestHead
    {
        public string Path { get; set; }
    }
    public class RequestBody
    {
        public CacheToWhere CacheToWhere { get; set; }
        public string CacheKey { get; set; }
        public object[] Paramters { get; set; }
    }
}
