using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
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

                //request.Paramters[Args[kv.Key]] = kv.Value;
            }
            return true;
        }

        public override string ToString()
        {
            return string.Format("{0}:/{1}/{2}", TemplateType.FullName, PathStart, string.Format(Format, Args));
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            return obj.ToString().Equals(ToString());
        }
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
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
