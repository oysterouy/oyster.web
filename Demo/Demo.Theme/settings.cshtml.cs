
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Settings : ISetting
    {
        public static readonly int _loadingTimeout = 300;
        public static readonly int i = 6;
        public static readonly string Name = "sdljflasdjfljwel,sd";
        public static readonly ISetting ii = new demotheme.Settings();
        public static readonly decimal aa = 6.978879M;

        
        static readonly List<Func<HttpContext, ITemplate>> routes = new List<Func<HttpContext, ITemplate>>();

        static readonly List<Func<HttpContext,bool>> filterBeforeRoute = new List<Func<HttpContext,bool>>();

        static readonly List<Func<HttpContext,ITemplate,Request,bool>> filterBeforeRequest = new  List<Func<HttpContext,ITemplate,Request,bool>>();

        static readonly List<Func<HttpContext,ITemplate,Request,Response,bool>> filterBeforeRander = new  List<Func<HttpContext,ITemplate,Request,Response,bool>>();

        static readonly List<Func<HttpContext,ITemplate,Request,Response,bool>> filterAfterRander = new List<Func<HttpContext,ITemplate,Request,Response,bool>>();

        static Settings()
        {
            //******** route setting *********//
            routes.Add((context) =>
{
    return new Index();
});


            //******** filter setting *********//

        }

        int ISetting.LoadingTimeout{get{ return _loadingTimeout;}}
        ITemplate ISetting.Route(HttpContext context)
        {
            foreach (var rt in routes)
            {
                var it = rt(context);
                if (it != null)
                    return it;
            }
            return null;
        }

        bool ISetting.BeforeRouteFilter(HttpContext context)
        {
            foreach (var filter in filterBeforeRoute)
            {
                if (!filter(context))
                    return false;
            }
            return true;
        }

        bool ISetting.BeforeRequestFilter(HttpContext context, ITemplate template, Request request)
        {
            foreach (var filter in filterBeforeRequest)
            {
                if (!filter(context, template, request))
                    return false;
            }
            return true;
        }

        bool ISetting.BeforeRanderFilter(HttpContext context, ITemplate template, Request request, Response response)
        {
            foreach (var filter in filterBeforeRander)
            {
                if (!filter(context, template, request, response))
                    return false;
            }
            return true;
        }

        bool ISetting.AfterRanderFilter(HttpContext context, ITemplate template, Request request, Response response)
        {
            foreach (var filter in filterBeforeRander)
            {
                if (!filter(context, template, request, response))
                    return false;
            }
            return true;
        }
    }
}
