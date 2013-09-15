using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using oyster.web.hosting;


namespace oyster.web
{
    public abstract class HostBase
    {
        public virtual ResponseInfo DoRequest(RequestHead header)
        {
            Request request = new Request(header) { Host = this };
            Response response = null;
            if (BeforeRouteFilter(request))
            {
                var temp = Route(request);
                if (temp == null)
                {
                    response = DefaultResponseManager.GetResponse(404);
                    goto outmethod;
                }
                request.Template = temp;
                var parms = temp.Init(request);
                if (request.IsError)
                    response = request.ErrorResponse;
                else
                {
                    request.Body.Paramters = parms;

                    response = RequestInternal(request);

                    response.Waiting();
                    response.Rander();
                    AfterRanderFilter(request, response);
                }
            }

        outmethod:

            return new ResponseInfo { Header = response.Head, Body = response.Body };
        }
        public abstract TemplateBase Route(Request request);
        public abstract bool BeforeRouteFilter(Request request);
        public abstract bool BeforeRequestFilter(Request request, Response response);
        public abstract bool BeforeRanderFilter(Request request, Response response);
        public abstract bool AfterRanderFilter(Request request, Response response);
        public abstract int LoadingTimeout { get; }

        internal Response RequestInternal(Request Request)
        {
            Response response = null;
            string outText = "";
            var cacheProvider = HostingHelper.GetRequestCacheProvider();
            if (cacheProvider != null && Request.Body.CacheKeySelect != null)
            {
                outText = cacheProvider.GetResponseText(Request.Body.CacheKeySelect, () =>
                           {
                               response = Request.Execute();
                               return response.GetOutPut();
                           });
            }
            if (!string.IsNullOrEmpty(outText))
            {
                response = new Response(outText);
            }
            else
            {
                //没有缓存设置时候
                response = Request.Execute();
            }


            return response;
        }
    }
}
