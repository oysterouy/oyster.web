
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Settings : HostBase
    {
        public static readonly int _loadingTimeout = 300;
        public static readonly int i = 6;
        public static readonly string Name = "sdljflasdjfljwel,sd";
        public static readonly decimal aa = 6.978879M;

        
        static readonly List<Func<Request, ITemplate>> routes = new List<Func<Request, ITemplate>>();

        static readonly List<Func<Request,bool>> filterBeforeRoute = new List<Func<Request,bool>>();

        static readonly List<Func<Request,bool>> filterBeforeRequest = new  List<Func<Request,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRander = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterAfterRander = new List<Func<Request,Response,bool>>();

        static Settings()
        {
            //******** route setting *********//
            routes.Add((Request) =>
{
    string path = Request.Head.Path.ToLower();
    path = path == "/" ? "/index" : path;
    if (path.Equals("/index"))
        return new Index();
    if (path.Equals("/login"))
        return new Index();
    return null;
});


            //******** filter setting *********//

        }

        public override int LoadingTimeout{get{ return _loadingTimeout;}}
        public override ITemplate Route(Request request)
        {
            foreach (var rt in routes)
            {
                var it = rt(request);
                if (it != null)
                    return it;
            }
            return null;
        }

        public override  bool BeforeRouteFilter(Request request)
        {
            foreach (var filter in filterBeforeRoute)
            {
                if (!filter(request))
                    return false;
            }
            return true;
        }

        public override  bool BeforeRequestFilter(Request request)
        {
            foreach (var filter in filterBeforeRequest)
            {
                if (!filter(request))
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
