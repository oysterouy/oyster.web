using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace oyster.web
{
    public interface ISetting
    {
        ITemplate Route(HttpContext context);
        bool BeforeRouteFilter(HttpContext context);
        bool BeforeRequestFilter(HttpContext context, ITemplate template, Request request);
        bool BeforeRanderFilter(HttpContext context, ITemplate template, Request request, Response response);
        bool AfterRanderFilter(HttpContext context, ITemplate template, Request request, Response response);
        int LoadingTimeout { get; }
    }
}
