using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web
{
    public class ParamtersInfo
    {
        public bool NeedCache { get; set; }
        public string CacheKey { get; set; }
        public object[] Paramsters { get; set; }
    }
    public abstract class TemplateBase
    {
    }
}
