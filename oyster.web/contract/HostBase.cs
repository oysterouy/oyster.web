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
        public virtual Response DoRequest(RequestHead header)
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
                    var reqData = temp.Init(request);
                    request.Body = reqData;
                    if (BeforeRequestFilter(request))
                    {
                        response = request.Execute();
                        if (BeforeRanderFilter(request, response))
                        {
                            response.Waiting();
                            response.Body = temp.Rander(response.Model);
                            AfterRanderFilter(request, response);
                        }
                    }
                }
            }

        outmethod:
            return response;
        }
        public abstract ITemplate Route(Request request);
        public abstract bool BeforeRouteFilter(Request request);
        public abstract bool BeforeRequestFilter(Request request);
        public abstract bool BeforeRanderFilter(Request request, Response response);
        public abstract bool AfterRanderFilter(Request request, Response response);
        public abstract int LoadingTimeout { get; }
    }
}
