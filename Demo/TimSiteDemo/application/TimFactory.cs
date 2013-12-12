using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using oyster.web.A.contract;

namespace TimSiteDemo.application
{
    public class TimFactory : ITim
    {
        public void ProcessRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public bool ProcessStaticResourceRequest(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public IHost GetHost(HttpContext context)
        {
            throw new NotImplementedException();
        }

        public Request InitRequest(HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}