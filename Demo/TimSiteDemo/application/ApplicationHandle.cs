using System;
using System.Web;
using oyster.web;
using timsitedemo;
using timsitedemo.templates.defaulttheme;
using timsitedemo.templates.newtheme;

namespace TimSiteDemo.application
{
    public class ApplicationHandle : IHttpHandler
    {
        public ApplicationHandle()
        {
            RouteManager.SetInstance(new DefaultRouteManager());
        }

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

        HostBase GetHost(HttpContext context)
        {
            string vparam = context.Request.Params["v"];
            if (!string.IsNullOrWhiteSpace(vparam) && vparam.Trim().ToLower().Equals("newtheme"))
            {
                return InstanceHelper<NewSettings>.Instance;
            }

            return InstanceHelper<DefaultSettings>.Instance;
        }

        public void ProcessRequest(HttpContext context)
        {
            //是静态资源
            if (ProcessStaticResource(context))
                return;

            var host = GetHost(context);

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

        bool ProcessStaticResource(HttpContext context)
        {
            string path = context.Request.Path.Trim().ToLower();
            if (!path.StartsWith("/context"))
                return false;

            var urlInfo = RouteManager.Instance.GetSrcUrlInfo(path);
            if (urlInfo == null)
                return false;

            string oldETag = context.Request.Headers["If-None-Match"];
            var response = context.Response;
            response.ContentType = urlInfo.ContentType;
            response.AppendHeader("ETAG", urlInfo.ETag);
            if (urlInfo.ETag == oldETag)
                response.StatusCode = 304;
            else
                response.WriteFile(urlInfo.RealPath);
            return true;
        }

        #endregion
    }
}
