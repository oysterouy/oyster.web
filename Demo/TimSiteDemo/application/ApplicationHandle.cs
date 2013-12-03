using System;
using System.Web;
using oyster.web;
using timsitedemo;

namespace TimSiteDemo.application
{
    public class ApplicationHandle : IHttpHandler
    {
        /// <summary>
        /// 您将需要在您网站的 web.config 文件中配置此处理程序，
        /// 并向 IIS 注册此处理程序，然后才能进行使用。有关详细信息，
        /// 请参见下面的链接: http://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpHandler Members

        public bool IsReusable
        {
            // 如果无法为其他请求重用托管处理程序，则返回 false。
            // 如果按请求保留某些状态信息，则通常这将为 false。
            get { return true; }
        }
        HostBase host = null;
        public void ProcessRequest(HttpContext context)
        {
            //在此写入您的处理程序实现。
            host = InstanceHelper<SiteSettings>.Instance;

            var header = oyster.web.hosting.HostingHelper.CreateHead(context);
            var resp = host.DoRequest(header);

            context.Response.StatusCode = resp.Header.StatusCode;
            if (!string.IsNullOrWhiteSpace(resp.Header.RedirectLocation))
                context.Response.RedirectLocation = resp.Header.RedirectLocation;
            resp.Header.Cookies.Add(new HttpCookie("aaa", "VVVV"));
            for (int i = 0; i < resp.Header.Cookies.Count; i++)
            {
                var ck = resp.Header.Cookies[i];
                context.Response.AppendCookie(ck);
            }

            context.Response.Write(resp.Body);

        }

        #endregion
    }
}
