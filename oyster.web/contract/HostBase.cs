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
            Request request = new Request { Head = header };
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
                if (BeforeRouteFilter(request))
                {
                    var parms = temp.Init(request);
                    request.Body.Paramters = parms;

                    if (BeforeRequestFilter(request))
                    {
                        response = request.Execute();
                        response.Waiting();
                        if (BeforeRanderFilter(request, response))
                        {
                            response.Rander();
                            AfterRanderFilter(request, response);
                        }
                    }
                }
            }

        outmethod:

            return new ResponseInfo { Header = response.Head, Body = response.Body };
        }
        public abstract TemplateBase Route(Request request);
        public abstract bool BeforeRouteFilter(Request request);
        public abstract bool BeforeRequestFilter(Request request);
        public abstract bool BeforeRanderFilter(Request request, Response response);
        public abstract bool AfterRanderFilter(Request request, Response response);
        public abstract int LoadingTimeout { get; }
    }
}
