using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.A.utility;
using oyster.web.A.implement;
using oyster.web.A.manage;

namespace oyster.web.A.contract
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

        public virtual void Route<T>(TimProcess host, string pathStart, string format, params string[] argNames)
            where T : TimTemplate
        {
            string s = string.Format(format, argNames);
            var r = new RouteInfo<T> { PathStart = pathStart, Format = format, Args = argNames };
            allRoutes.Add(r);

            List<RouteInfo> routeLs = null;
            if (!templateRoutes.TryGetValue(host, out routeLs))
            {
                routeLs = new List<RouteInfo>();
                templateRoutes.Add(host, routeLs);
            }
            routeLs.Add(r);

            KeyValueCollection<string, List<RouteInfo>> tempRoutes = null;
            if (!urlTemplateRoutes.TryGetValue(host, out tempRoutes))
            {
                tempRoutes = new KeyValueCollection<string, List<RouteInfo>>();
                urlTemplateRoutes.Add(host, tempRoutes);
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

        public virtual void Route(TimProcess host, Func<Request, TimTemplate> route)
        {
            allRouteFuncs.Add(route);
            List<Func<Request, TimTemplate>> routeFuncs = null;
            if (!templaterouteFuncs.TryGetValue(host, out routeFuncs))
            {
                routeFuncs = new List<Func<Request, TimTemplate>>();
                templaterouteFuncs.Add(host, routeFuncs);
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

    public abstract class RouteInfo
    {
        public Type TemplateType { get; protected set; }
        public string PathStart { get; set; }
        public string Format { get; set; }
        public string[] Args { get; set; }

        public virtual bool IsMatch(Request request)
        {
            string url = request.Path;
            if (!url.StartsWith(PathStart))
                return false;

            int fmtIdx = 0;
            Dictionary<int, string> argsDic = new Dictionary<int, string>();
            for (int i = 0; i < url.Length; i++)
            {
                if (fmtIdx >= Format.Length)
                    return false;

                char c = Format[fmtIdx];
                if (c == '{')
                {
                    int argEnd = Format.IndexOf('}', fmtIdx);
                    if (argEnd < 0)
                        throw new Exception("Route format is error!");

                    int argIndex = -1;
                    if (!Int32.TryParse(Format.Substring(fmtIdx + 1, argEnd - fmtIdx - 1), out argIndex))
                    {
                        throw new Exception("Route format is error parameter must be {Number}!");
                    }

                    int nextArgBegin = Format.IndexOf('{', argEnd);
                    string argAfterChars = "";
                    int urlArgEnd = -1;
                    if (nextArgBegin < 0)
                    {
                        //argAfterChars = Format.Substring(argEnd + 1);
                        urlArgEnd = url.Length;
                    }
                    else
                    {
                        argAfterChars = Format.Substring(argEnd + 1, nextArgBegin - argEnd - 1);
                        urlArgEnd = url.IndexOf(argAfterChars, i);
                    }
                    if (urlArgEnd < 0)
                        return false;

                    string arg = url.Substring(i, urlArgEnd - i);
                    if (!argsDic.ContainsKey(argIndex))
                        argsDic.Add(argIndex, arg);

                    fmtIdx = argEnd;
                    i = urlArgEnd - 1;
                }
                else if (c != url[i])
                    return false;
                fmtIdx++;
            }
            if (argsDic.Count > 0 && (Args == null || Args.Length != argsDic.Count))
                return false;
            foreach (var kv in argsDic)
            {
                if (Args.Length < kv.Key)
                    throw new Exception("Route format is error,{Number} Number is genetate then argNames Length!");

                request.Paramters[Args[kv.Key]] = kv.Value;
            }
            return true;
        }
    }
    public class RouteInfo<T> : RouteInfo
          where T : TimTemplate
    {
        public RouteInfo()
        {
            TemplateType = typeof(T);
        }
    }
}
