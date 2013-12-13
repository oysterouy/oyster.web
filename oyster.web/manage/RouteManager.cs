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
        static readonly KeyValueCollection<TimProcess, List<RouteInfo>> templateRoutes = new KeyValueCollection<TimProcess, List<RouteInfo>>();
        static readonly KeyValueCollection<TimProcess, KeyValueCollection<string, List<RouteInfo>>> urlTemplateRoutes = new KeyValueCollection<TimProcess, KeyValueCollection<string, List<RouteInfo>>>();
        static readonly KeyValueCollection<TimProcess, List<Func<Request, TimTemplate>>> templaterouteFuncs = new KeyValueCollection<TimProcess, List<Func<Request, TimTemplate>>>();

        string GetTemplateTypeKey(Type tp, int argCount)
        {
            return string.Format("{0}-[{1}]", tp.FullName, argCount);
        }

        public virtual void Route<T>(TimProcess process, string pathStart, string format, params string[] argNames)
            where T : TimTemplate
        {
            string s = string.Format(format, argNames);
            var r = new RouteInfo<T> { PathStart = pathStart, Format = format, Args = argNames };
            allRoutes.Add(r);

            List<RouteInfo> routeLs = null;
            if (!templateRoutes.TryGetValue(process, out routeLs))
            {
                routeLs = new List<RouteInfo>();
                templateRoutes.Add(process, routeLs);
            }
            routeLs.Add(r);

            KeyValueCollection<string, List<RouteInfo>> tempRoutes = null;
            if (!urlTemplateRoutes.TryGetValue(process, out tempRoutes))
            {
                tempRoutes = new KeyValueCollection<string, List<RouteInfo>>();
                urlTemplateRoutes.Add(process, tempRoutes);
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

        public virtual void Route(TimProcess process, Func<Request, TimTemplate> route)
        {
            allRouteFuncs.Add(route);
            List<Func<Request, TimTemplate>> routeFuncs = null;
            if (!templaterouteFuncs.TryGetValue(process, out routeFuncs))
            {
                routeFuncs = new List<Func<Request, TimTemplate>>();
                templaterouteFuncs.Add(process, routeFuncs);
            }
            routeFuncs.Add(route);
        }

        public virtual TimTemplate Match(Request request)
        {
            List<RouteInfo> routes = null;
            List<Func<Request, TimTemplate>> routeFuncs = null;
            var process = TimProcessContext.GetProcess();
            if (process != null)
            {
                templateRoutes.TryGetValue(process, out routes);
                templaterouteFuncs.TryGetValue(process, out routeFuncs);
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
            var process = TimProcessContext.GetProcess();
            string k = GetTemplateTypeKey(typeof(T), args == null ? 0 : args.Length);

            KeyValueCollection<string, List<RouteInfo>> urlRouteDic = null;
            List<RouteInfo> rls = null;
            if (
                !urlTemplateRoutes.TryGetValue(process, out urlRouteDic) ||
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
