using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Host
{
    public class MainHandle : oyster.web.MainHandle
    {

        public MainHandle()
        {
            var dic = SitesManager.LoadSiteTemplate(HttpContext.Current.Server.MapPath("~/templateroot"), HttpContext.Current.Server.MapPath("~/sites"));

            ApplicationDic = dic;
        }
        protected override oyster.web.AppDomainDTO MapApplication(HttpContext context)
        {
            return ApplicationDic != null ? ApplicationDic.Keys.First() : null;
        }
    }
}