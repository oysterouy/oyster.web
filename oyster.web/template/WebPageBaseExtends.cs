using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;

namespace oyster.web
{
    public static class WebPageBaseExtends
    {
        public static string Echo(this WebPageBase page, string format, params object[] args)
        {
            return "";
        }
    }
}

namespace System.Web.WebPages
{
    public class WebPageBase
    {
        public int AA()
        {
            return 0;
        }
    }
}
