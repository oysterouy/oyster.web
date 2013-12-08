
namespace timsitedemo.templates.newtheme
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using timsitedemo.templates.newtheme;

    public class NewSettings : HostBase
    {
        static HostBase This = InstanceHelper<NewSettings>.Instance;
        public static readonly int _loadingTimeout = 200;
        public static readonly string _templateStaticResourceDir = "/templates/defaulttheme/context";

        static readonly List<Func<Request,bool>> filterBeforeRoute = new List<Func<Request,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRequest = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRander = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterAfterRander = new List<Func<Request,Response,bool>>();

        static NewSettings()
        {
            //******** route setting *********//
             RouteManager.Instance.Route<Index>(This,"/", "/");
            RouteManager.Instance.Route<Index>(This,"/index", "/index/{0}-_-{1}/", "name", "age");
            RouteManager.Instance.Route<Index>(This,"/idx", "/idx/{0}", "n");
            RouteManager.Instance.Route(This,(request) =>
{
    return InstanceHelper<Index>.Instance;
});


            //******** filter setting *********//
 
        }

        public override int LoadingTimeout{get{ return _loadingTimeout;}}
        public override string TemplateStaticResourceDir{get{ return _templateStaticResourceDir;}}
       
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
