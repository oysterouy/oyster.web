using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web.hosting
{
    public class HostingHelper
    {
        public static RequestHead CreateHead(HttpContext context)
        {
            return new RequestHead
            {
                Path = context.Request.Path,
                Method = context.Request.HttpMethod,
                Paramters = context.Request.Params,
                Cookies = context.Request.Cookies,
            };
        }

        public static IRequestCache GetRequestCacheProvider()
        {
            return null;
        }
    }
}
