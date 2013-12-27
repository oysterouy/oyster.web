﻿using System;
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
            if (ProcessStaticResourceRequest(context))
                return;

            var host = GetHost(context);
            var request = CreateRequest(context.Request);
            var response = host.Execute(request);

            //todo:output response
        }

        protected virtual bool ProcessStaticResourceRequest(HttpContext context)
        {
            string path = context.Request.Path.Trim().ToLower();
            if (!path.StartsWith(StaticResourceManager.ResourceUrlStart))
                return false;

            var urlInfo = RouteManager.Instance.GetSrcUrlInfo(context.Request.Url);
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
        }
    }
}
