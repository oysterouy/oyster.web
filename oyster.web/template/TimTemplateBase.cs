using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;

namespace oyster.web
{
    public abstract class TimTemplateBase
    {
        public string Echo(string format, params object[] args)
        {
            return "";
        }
    }
}
