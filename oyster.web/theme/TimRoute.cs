using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.manage;
using System.Collections.Concurrent;

namespace oyster.web
{
    public class TimRoute
    {
        static ConcurrentDictionary<string, List<RouteInfo>> TemplateRoutes { get; set; }
        static ConcurrentDictionary<string, List<Func<Request, TimTemplate>>> TemplateFuncRoutes { get; set; }
        static TimRoute()
        {
            TemplateRoutes = new ConcurrentDictionary<string, List<RouteInfo>>();
            TemplateFuncRoutes = new ConcurrentDictionary<string, List<Func<Request, TimTemplate>>>();
        }
        internal TimRoute(TimTheme theme)
            : this(theme, null)
        { }
        internal TimRoute(TimTheme theme, TimRoute baseRoute)
        {
            Theme = theme;
            BaseRoute = baseRoute;
        }

        internal void SetBase(TimRoute baseRoute)
        {
            BaseRoute = baseRoute;
        }

        public TimTheme Theme { get; protected set; }
        public TimRoute BaseRoute { get; protected set; }
        public List<RouteInfo> Routes
        {
            get
            {
                List<RouteInfo> ls = null;
                TemplateRoutes.TryGetValue(GetThemeRouteKey(), out ls);
                return ls ?? new List<RouteInfo>();
            }
        }
        string GetThemeRouteKey()
        {
            return Theme.GetType().FullName;
        }
        public void Add<T>(string pathStart, string format, params string[] argNames)
            where T : TimTemplate
        {
            var route = new RouteInfo<T>
            {
                PathStart = pathStart,
                Format = format,
                Args = argNames
            };
            var ls = new List<RouteInfo> { route };
            TemplateRoutes.AddOrUpdate(GetThemeRouteKey(), ls, (k, storeLs) =>
            {
                foreach (var r in ls)
                {
                    if (!storeLs.Contains(r))
                        storeLs.Add(r);
                }
                return storeLs;
            });
        }
        public void Add(Func<Request, TimTemplate> route)
        {
            var ls = new List<Func<Request, TimTemplate>>();
            ls.Add(route);

            TemplateFuncRoutes.AddOrUpdate(GetThemeRouteKey(), ls, (k, storeLs) =>
             {
                 storeLs.AddRange(ls);
                 return storeLs;
             });
        }
        RouteInfo MatchRoute(Request request)
        {
            foreach (var r in Routes)
            {
                if (r.IsMatch(request))
                    return r;
            }
            return null;
        }

        public virtual TimTemplate Match(Request request)
        {
            var r = MatchRoute(request);
            if (r != null)
                return InstanceHelper.GetInstance(r.TemplateType) as TimTemplate;

            List<Func<Request, TimTemplate>> routeFuncs = null;
            if (TemplateFuncRoutes.TryGetValue(GetThemeRouteKey(), out routeFuncs))
            {
                foreach (var f in routeFuncs)
                {
                    var temp = f(request);
                    if (temp != null)
                        return temp;
                }
            }
            if (BaseRoute != null)
                return BaseRoute.Match(request);
            return null;
        }

        public virtual string Url<T>(string pathStart, object[] args)
           where T : TimTemplate
        {
            RouteInfo defaultRoute = null;
            RouteInfo matchPathStartRoute = null;
            RouteInfo matchRoute = null;
            foreach (var rr in Routes)
            {
                if (defaultRoute == null && rr.TemplateType == typeof(T))
                    defaultRoute = rr;

                if (matchPathStartRoute == null && rr.PathStart == pathStart)
                    matchPathStartRoute = rr;

                if (rr.PathStart == pathStart
                    && (rr.Args == null ? 0 : rr.Args.Length) == (args == null ? 0 : args.Length))
                {
                    matchRoute = rr;
                    break;
                }
            }
            if (matchRoute != null)
                return string.Format(matchRoute.Format, args);

            throw new Exception(string.Format("Template:{0} do`nt have {1} count args route!",
                   typeof(T).FullName, args == null ? 0 : args.Length));
        }

        public virtual string Src(string fileName)
        {
            return StaticResourceManager.GetResourceUrl(fileName);
        }
    }
}
