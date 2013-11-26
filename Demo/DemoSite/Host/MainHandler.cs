using System;
using System.Web;
using oyster.web.hosting;
using oyster.web;
using demosite.Themes.WhiteBlue;

namespace DemoSite.Host
{
    public class MainHandler : IHttpHandler
    {
        /// <summary>
        /// You will need to configure this handler in the web.config file of your 
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
        static readonly HostBase Host = new ASettings();
        public void ProcessRequest(HttpContext context)
        {
            var reqheader = HostingHelper.CreateHead(context);
            var resp = Host.DoRequest(reqheader);
            context.Response.StatusCode = resp.Header.StatusCode;
            context.Response.Write(resp.Body);
        }

        #endregion
    }
}
