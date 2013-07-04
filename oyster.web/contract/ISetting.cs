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
        bool Filter(FilterOnEnum fon, HttpContext context, ITemplate it, StringBuilder str);
    }
}
