
namespace demosite.Themes.WhiteBlue
{
    using oyster.web;
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Settings : HostBase
    {
        public static readonly int _loadingTimeout = 200;

        
        static readonly List<Func<Request, TemplateBase>> routes = new List<Func<Request, TemplateBase>>();

        static readonly List<Func<Request,bool>> filterBeforeRoute = new List<Func<Request,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRequest = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterBeforeRander = new  List<Func<Request,Response,bool>>();

        static readonly List<Func<Request,Response,bool>> filterAfterRander = new List<Func<Request,Response,bool>>();

        static Settings()
        {
            //******** route setting *********//
            routes.Add((Request) =>
{
    string path = Request.Head.Path;
    path = (string.IsNullOrWhiteSpace(path) || path == "/") ? "/index" : path;
    switch (path)
    {
        case "/index":
            return new demosite.Themes.WhiteBlue.Index();
    }

    return null;
});


            //******** filter setting *********//
            filterBeforeRequest.Add((request, response) =>
{
    if (response.Model.Head == null)
    {
        response.Model.Head = new DynamicModel();
        response.Model.Head.description = "默认描述";
        response.Model.Head.keywords = "默认关键词";
        response.Model.Head.title = "默认标题";
    }
    return true;
});

        }

        public override int LoadingTimeout{get{ return _loadingTimeout;}}
        public override TemplateBase Route(Request request)
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
