using System;
using System.Web;
using oyster.web;

namespace Demo.Host
{
    public class AppHandle : IHttpHandler
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

        ISetting GetAppHost(HttpContext webContext)
        {
            return null;
        }
        public void ProcessRequest(HttpContext context)
        {
            var host = GetAppHost(context);
            Request req = null;
            Response resp = null;
            if (host.BeforeRouteFilter(context))
            {
                var temp = host.Route(context);
                if (host.BeforeRouteFilter(context))
                {
                    req = temp.Init(context);
                    if (host.BeforeRequestFilter(context, temp, req))
                    {
                        resp = req.Execute();
                        if (host.BeforeRanderFilter(context, temp, req, resp))
                        {
                            resp.Waiting(host.LoadingTimeout);
                            resp.Body = temp.Rander(resp.Model);
                            host.AfterRanderFilter(context, temp, req, resp);
                        }
                    }
                }
            }

            context.Response.Write(resp.Body);
        }

        #endregion
    }
}
