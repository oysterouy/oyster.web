using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using oyster.web.manage;

namespace oyster.web.host
{
    public class TimEngine
    {
        private TimEngine()
        { }
        static TimEngine _instance;
        public static TimEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimEngine();
                }
                return _instance;
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            var host = GetHost(context);
            if (ProcessStaticResourceRequest(host, context))
                return;

            var request = CreateRequest(context.Request);
            var response = host.Execute(request);

            context.Response.StatusCode = response.StatusCode;
            if (!string.IsNullOrWhiteSpace(response.RedirectLocation))
                context.Response.RedirectLocation = response.RedirectLocation;

            for (int i = 0; i < response.Headers.Count; i++)
            {
                var hd = response.Headers[i];
                context.Response.AppendHeader(hd.Key, hd.Value);
            }
            for (int i = 0; i < response.Cookies.Count; i++)
            {
                var ck = response.Cookies[i];
                context.Response.AppendCookie(ck);
            }
            context.Response.Write(response.Body);
            context.Response.Flush();
        }

        protected virtual bool ProcessStaticResourceRequest(TimHost host, HttpContext context)
        {
            string path = context.Request.Path.Trim().ToLower();
            if (!path.StartsWith(StaticResourceManager.ResourceUrlStart))
                return false;

            var urlInfo = StaticResourceManager.GetResourceUrlInfo(host, context.Request.Url);
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

        internal virtual TimHost GetHost(System.Web.HttpContext context)
        {
            if (getHostFunc == null)
                throw new Exception("You must implement TimEngine.GetHost virtual method or Invoke TimEngine.SetHostFunc!");
            return getHostFunc(context);
        }

        Func<HttpContext, TimHost> getHostFunc = null;
        public TimEngine SetHostFunc(Func<HttpContext, TimHost> func)
        {
            getHostFunc = func;
            return this;
        }
        Request CreateRequest(HttpRequest request)
        {
            var timRequest = new Request();
            InitRequest(request, timRequest);
            return timRequest;
        }

        protected virtual void InitRequest(HttpRequest request, Request timRequest)
        {
            timRequest.RequestUrl = request.Url;
            timRequest.RequestUrlReferrer = request.UrlReferrer;
        }
    }
}
