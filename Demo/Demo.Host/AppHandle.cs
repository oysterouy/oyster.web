using System;
using System.Web;
using oyster.web;
using System.IO;
using System.Collections.Generic;
using oyster.web.hosting;
using System.Configuration;

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
        static object lockLoaderDomain = new object();
        static DateTime lastLoadTime = DateTime.MinValue;
        static List<HostProxy> settings = new List<HostProxy>();
        static void LoadTemplateModules(string tempRoot, string libRoot)
        {
            if (lastLoadTime.AddMinutes(5) < DateTime.Now)
            {
                lock (lockLoaderDomain)
                {
                    if (lastLoadTime.AddMinutes(5) < DateTime.Now)
                    {
                        lastLoadTime = DateTime.Now;
                        var tempFs = Directory.GetFiles(tempRoot, "*.dll", SearchOption.AllDirectories);
                        var ls = new List<string>();
                        foreach (var fs in tempFs)
                        {
                            var loader = HostAssermblyLoader.CreateLoader(Path.GetFileNameWithoutExtension(fs),
                               libRoot, fs, AppDomain.CurrentDomain.BaseDirectory);

                            if (loader != null)
                                settings.Add(loader.Application.HostSetting);
                        }
                    }
                }
            }
        }

        static AppHandle()
        {
            string cnfTempRoot = ConfigurationManager.AppSettings["TemplateRoot"];
            string tempRoot = HttpContext.Current.Server.MapPath(cnfTempRoot);

            string cnfLibRoot = ConfigurationManager.AppSettings["LibRoot"];
            string libRoot = HttpContext.Current.Server.MapPath(cnfLibRoot);

            LoadTemplateModules(tempRoot, libRoot);
        }

        HostProxy GetAppHost(HttpContext webContext)
        {
            foreach (var set in settings)
            {
                return set;
            }
            return null;
        }
        public void ProcessRequest(HttpContext context)
        {
            var host = GetAppHost(context);
            var reqheader = new RequestHead { Path = context.Request.Path };
            var resp = host.DoRequest(reqheader);
            context.Response.StatusCode = resp.Head.StatusCode;
            context.Response.Write(resp.Body);
        }

        #endregion
    }
}
