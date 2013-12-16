using System;
using System.Collections.Generic;
using oyster.web.manage;

namespace oyster.web.manage
{
    public class RouteManager
    {
        static RouteManager instance;
        public static RouteManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new RouteManager();
                return instance;
            }
        }
        public static void SetInstance(RouteManager mgr)
        {
            instance = mgr;
        }


        static readonly List<RouteInfo> allRoutes = new List<RouteInfo>();
        static readonly List<Func<Request, TimTemplate>> allRouteFuncs = new List<Func<Request, TimTemplate>>();
        static readonly KeyValueCollection<TimTheme, List<RouteInfo>> templateRoutes = new KeyValueCollection<TimTheme, List<RouteInfo>>();
        static readonly KeyValueCollection<TimTheme, KeyValueCollection<string, List<RouteInfo>>> urlTemplateRoutes = new KeyValueCollection<TimTheme, KeyValueCollection<string, List<RouteInfo>>>();
        static readonly KeyValueCollection<TimTheme, List<Func<Request, TimTemplate>>> templaterouteFuncs = new KeyValueCollection<TimTheme, List<Func<Request, TimTemplate>>>();

        string GetTemplateTypeKey(Type tp, int argCount)
        {
            return string.Format("{0}-[{1}]", tp.FullName, argCount);
        }

        public virtual void Route<TTheme, TTemplate>(string pathStart, string format, params string[] argNames)
            where TTemplate : TimTemplate
            where TTheme : TimTheme
        {
            var theme = InstanceHelper<TTheme>.Instance;
            string s = string.Format(format, argNames);
            var r = new RouteInfo<TTemplate> { PathStart = pathStart, Format = format, Args = argNames };
            allRoutes.Add(r);

            List<RouteInfo> routeLs = null;
            if (!templateRoutes.TryGetValue(theme, out routeLs))
            {
                routeLs = new List<RouteInfo>();
                templateRoutes.Add(theme, routeLs);
            }
            routeLs.Add(r);

            KeyValueCollection<string, List<RouteInfo>> tempRoutes = null;
            if (!urlTemplateRoutes.TryGetValue(theme, out tempRoutes))
            {
                tempRoutes = new KeyValueCollection<string, List<RouteInfo>>();
                urlTemplateRoutes.Add(theme, tempRoutes);
            }
            List<RouteInfo> rls = null;
            string k = GetTemplateTypeKey(r.TemplateType, r.Args.Length);
            if (!tempRoutes.TryGetValue(k, out rls))
            {
                rls = new List<RouteInfo>();
                tempRoutes.Add(k, rls);
            }
            rls.Add(r);
        }

        public virtual void Route<TTheme>(Func<Request, TimTemplate> route)
         where TTheme : TimTheme
        {
            var theme = InstanceHelper<TTheme>.Instance;
            allRouteFuncs.Add(route);
            List<Func<Request, TimTemplate>> routeFuncs = null;
            if (!templaterouteFuncs.TryGetValue(theme, out routeFuncs))
            {
                routeFuncs = new List<Func<Request, TimTemplate>>();
                templaterouteFuncs.Add(theme, routeFuncs);
            }
            routeFuncs.Add(route);
        }

        public virtual TimTemplate Match(Request request)
        {
            List<RouteInfo> routes = null;
            List<Func<Request, TimTemplate>> routeFuncs = null;
            var theme = TimProcessContext.GetProcess();
            if (theme != null)
            {
                templateRoutes.TryGetValue(theme.Theme, out routes);
                templaterouteFuncs.TryGetValue(theme.Theme, out routeFuncs);
            }
            routes = routes ?? allRoutes;

            foreach (var r in routes)
            {
                if (r.IsMatch(request))
                    return InstanceHelper.GetInstance(r.TemplateType) as TimTemplate;
            }

            routeFuncs = routeFuncs ?? allRouteFuncs;

            foreach (var f in routeFuncs)
            {
                var temp = f(request);
                if (temp != null)
                    return temp;
            }
            return null;
        }

        public virtual string Url<T>(object[] args)
            where T : TimTemplate
        {
            return Url<T>(null, args);
        }

        public virtual string Url<T>(string pathStart, object[] args)
            where T : TimTemplate
        {
            var theme = TimProcessContext.GetProcess();
            string k = GetTemplateTypeKey(typeof(T), args == null ? 0 : args.Length);

            KeyValueCollection<string, List<RouteInfo>> urlRouteDic = null;
            List<RouteInfo> rls = null;
            if (
                !urlTemplateRoutes.TryGetValue(theme.Theme, out urlRouteDic) ||
                !urlRouteDic.TryGetValue(k, out rls) ||
                rls.Count < 1)
            {
                throw new Exception(string.Format("Template:{0} do`nt have {1} count args route!",
                    typeof(T).FullName, args == null ? 0 : args.Length));
            }
            RouteInfo r = null;
            if (string.IsNullOrWhiteSpace(pathStart))
                r = rls[0];
            else
            {
                foreach (var rr in rls)
                {
                    if (rr.PathStart == pathStart)
                    {
                        r = rr;
                        break;
                    }
                }
            }

            return string.Format(r.Format, args);
        }

        public virtual string Src(string fileName)
        {
            return StaticResourceManager.GetResourceUrl(fileName);
        }

        public virtual ResourceUrlInfo GetSrcUrlInfo(string fileName)
        {
            return StaticResourceManager.GetResourceUrlInfo(fileName);
        }
    }
}
