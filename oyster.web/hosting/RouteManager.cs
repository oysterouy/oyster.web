using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.define;

namespace oyster.web
{
    public class RouteManager
    {
        static readonly List<RouteInfo> routes = new List<RouteInfo>();
        static readonly KeyValueCollection<string, List<RouteInfo>> templateRoutes = new KeyValueCollection<string, List<RouteInfo>>();
        static readonly List<Func<Request, TemplateBase>> routeFuncs = new List<Func<Request, TemplateBase>>();

        static string GetTemplateTypeKey(Type tp, int argCount)
        {
            return string.Format("{0}-[{1}]", tp.FullName, argCount);
        }

        public static void Route<T>(string pathStart, string format, params string[] argNames)
            where T : TemplateBase
        {
            string s = string.Format(format, argNames);
            var r = new RouteInfo<T> { PathStart = pathStart, Format = format, Args = argNames };
            routes.Add(r);
            List<RouteInfo> rls = null;
            string k = GetTemplateTypeKey(r.TemplateType, r.Args.Length);
            if (!templateRoutes.TryGetValue(k, out rls))
            {
                rls = new List<RouteInfo>();
                templateRoutes.Add(k, rls);
            }
            rls.Add(r);
        }

        public static void Route(Func<Request, TemplateBase> route)
        {
            routeFuncs.Add(route);
        }

        public static TemplateBase Match(Request request)
        {
            foreach (var r in routes)
            {
                if (r.IsMatch(request))
                    return InstanceHelper.GetInstance(r.TemplateType) as TemplateBase;
            }
            foreach (var f in routeFuncs)
            {
                var temp = f(request);
                if (temp != null)
                    return temp;
            }
            return null;
        }
        public static string Url<T>(object[] args)
            where T : TemplateBase
        {
            return Url<T>(null, args);
        }
        public static string Url<T>(string pathStart, object[] args)
            where T : TemplateBase
        {
            string k = GetTemplateTypeKey(typeof(T), args == null ? 0 : args.Length);

            List<RouteInfo> rls = null;
            if (!templateRoutes.TryGetValue(k, out rls) || rls.Count < 1)
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
    }

    public abstract class RouteInfo
    {
        public Type TemplateType { get; protected set; }
        public string PathStart { get; set; }
        public string Format { get; set; }
        public string[] Args { get; set; }

        public virtual bool IsMatch(Request request)
        {
            string url = request.Head.Path;
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

                request.Head.Paramters[Args[kv.Key]] = kv.Value;
            }
            return true;
        }
    }
    public class RouteInfo<T> : RouteInfo
          where T : TemplateBase
    {
        public RouteInfo()
        {
            TemplateType = typeof(T);
        }
    }
}
