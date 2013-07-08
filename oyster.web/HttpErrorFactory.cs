using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web
{
    public class HttpErrorFactory
    {
        public static void Err404(HttpContext context)
        {
            context.Response.Clear();
            context.Response.StatusCode = 404;
            context.Response.Write("page no found!");
        }
    }
}
