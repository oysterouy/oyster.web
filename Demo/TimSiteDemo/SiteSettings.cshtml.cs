
namespace timsitedemo
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class SiteSettings : HostBase
    {
        public static readonly int _loadingTimeout = 200;

        static readonly List<Func<Request,bool>> filterBeforeRoute = new List<Func<Request,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRequest = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRander = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterAfterRander = new List<Func<Request,Response,bool>>();

        static SiteSettings()
        {
            //******** route setting *********//
             RouteManager.Route<timsitedemo.Index>("/", "/");
            RouteManager.Route<timsitedemo.Index>("/index", "/index/{0}-_-{1}/", "name", "age");
            RouteManager.Route<timsitedemo.Index>("/idx", "/idx/{0}", "n");
            RouteManager.Route((request) =>
{
    return InstanceHelper<timsitedemo.Index>.Instance;
});


            //******** filter setting *********//
 
        }

        public override int LoadingTimeout{get{ return _loadingTimeout;}}
       
        public override  bool BeforeRouteFilter(Request request)
        {
            foreach (var filter in filterBeforeRoute)
            {
                if (!filter(request))
                    return false;
            }
            return true;
        }

        public override  bool BeforeRequestFilter(Request request, Response response)
        {
            foreach (var filter in filterBeforeRequest)
            {
                if (!filter(request,response))
                    return false;
            }
            return true;
        }

        public override  bool BeforeRanderFilter(Request request, Response response)
        {
            foreach (var filter in filterBeforeRander)
            {
                if (!filter(request, response))
                    return false;
            }
            return true;
        }

        public override  bool AfterRanderFilter(Request request, Response response)
        {
            foreach (var filter in filterBeforeRander)
            {
                if (!filter(request, response))
                    return false;
            }
            return true;
        }
    }
}
