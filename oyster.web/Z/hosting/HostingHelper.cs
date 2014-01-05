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
            var hd = new RequestHead
            {
                Path = context.Request.Path,
                Method = context.Request.HttpMethod,
            };
            hd.Paramters.Add(context.Request.Params);
            foreach (string key in context.Request.Cookies.AllKeys)
            {
                var ck = context.Request.Cookies[key];
                hd.Cookies.Add(ck);
            }

            return hd;
        }

        public static IRequestCache GetRequestCacheProvider()
        {
            return null;
        }
    }
}
