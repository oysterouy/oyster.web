using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace oyster.web.codegenerator
{
    class RazorResolverSettings : IResolve
    {
        public RazorResolverSettings(string codeText, string classFullName)
        {
            _codeText = codeText;
            string nameSpace = System.IO.Path.GetFileNameWithoutExtension(classFullName);
            string className = System.IO.Path.GetExtension(classFullName);
            className = className.StartsWith(".") ? className.Substring(1) : className;
            className = className.Substring(0, 1).ToUpper() + className.Substring(1);

            NameSpace = nameSpace;
            ClassName = className;
        }
        string _codeText;
        string NameSpace { get; set; }
        string ClassName { get; set; }

        public string DoResolve()
        {
            Regex regField = new Regex("TemplateHelper\\.Config\\(\\(([^\\)]+)\\)\\s*=>\\s*(.*)\\)", RegexOptions.Singleline);

            Regex regRoute = new Regex("TemplateHelper\\.Route\\((.*)\\)", RegexOptions.Singleline);

            Regex regFilter = new Regex("TemplateHelper\\.Filter(^[\\(])+\\s*\\((.*)\\)", RegexOptions.Singleline);

            var r = new RazorResolver(_codeText);

            List<string> fieldCodeList = new List<string>();
            List<string> routeCodeList = new List<string>();

            Dictionary<string, List<string>> filterCodeDic = new Dictionary<string, List<string>>();

            foreach (var code in r.OutCodeList.Values)
            {
                if (code.Contains("TemplateHelper.Config("))
                {
                    var m = regField.Match(code);
                    if (m.Success && m.Groups.Count > 2)
                    {
                        fieldCodeList.Add(string.Format("public static readonly {0} = {1};", m.Groups[1].Value, m.Groups[2].Value));
                    }
                }
                else if (code.Contains("TemplateHelper.Route("))
                {
                    var m = regRoute.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        routeCodeList.Add(m.Groups[1].Value);
                    }
                }
                else if (code.Contains("TemplateHelper.Filter"))
                {
                    var m = regFilter.Match(code);
                    if (m.Success && m.Groups.Count > 2)
                    {
                        string fon = m.Groups[1].Value.Trim();
                        List<string> ls = null;
                        if (!filterCodeDic.TryGetValue(fon, out ls))
                        {
                            ls = new List<string>();
                            filterCodeDic.Add(fon, ls);
                        }
                        ls.Add(m.Groups[2].Value);
                    }
                }
            }

            string codeUsing = "";
            r.UsingNames.Sort();
            foreach (string s in r.UsingNames)
            {
                codeUsing += string.Format("    using {0};\r\n", s);
            }

            StringBuilder codeFields = new StringBuilder();
            foreach (var f in fieldCodeList)
            {
                codeFields.AppendFormat("        {0}\r\n", f);
            }

            StringBuilder addcodeRoute = new StringBuilder();
            foreach (var f in routeCodeList)
            {
                addcodeRoute.AppendFormat("           routes.Add({0});\r\n", f);
            }

            StringBuilder addcodeFilter = new StringBuilder();
            int i = 0;
            foreach (string fon in filterCodeDic.Keys)
            {
                var ls = filterCodeDic[fon];
                foreach (var f in ls)
                {
                    addcodeFilter.AppendFormat("            filter{0}.Add({0});\r\n", fon, f);
                }
            }


            string codetxt = @"
namespace " + NameSpace + @"
{
    using oyster.web;
" + codeUsing + @"
    public class " + ClassName + @" : ISetting
    {
" + codeFields.ToString() +
  @"
        
        static readonly List<Func<HttpContext, ITemplate>> routes = new List<Func<HttpContext, ITemplate>>();

        static readonly List<Func<HttpContext,bool>> filterBeforeRoute = new List<Func<HttpContext,bool>>();

        static readonly List<Func<HttpContext,ITemplate,Request,bool>> filterBeforeRequest = new  List<Func<HttpContext,ITemplate,Request,bool>>();

        static readonly List<Func<HttpContext,ITemplate,Request,Response,bool>> filterBeforeRander = new  List<Func<HttpContext,ITemplate,Request,Response,bool>>();

        static readonly List<Func<HttpContext,ITemplate,Request,Response,bool>> filterAfterRander = new List<Func<HttpContext,ITemplate,Request,Response,bool>>();

        static Settings()
        {
            //******** route setting *********//
 " + addcodeRoute + @"

            //******** filter setting *********//
" + addcodeFilter + @"
        }

        int ISetting.LoadingTimeout{get{ return _loadingTimeout;}}
        ITemplate ISetting.Route(HttpContext context)
        {
            foreach (var rt in routes)
            {
                var it = rt(context);
                if (it != null)
                    return it;
            }
            return null;
        }

        bool ISetting.BeforeRouteFilter(HttpContext context)
        {
            foreach (var filter in filterBeforeRoute)
            {
                if (!filter(context))
                    return false;
            }
            return true;
        }

        bool ISetting.BeforeRequestFilter(HttpContext context, ITemplate template, Request request)
        {
            foreach (var filter in filterBeforeRequest)
            {
                if (!filter(context, template, request))
                    return false;
            }
            return true;
        }

        bool ISetting.BeforeRanderFilter(HttpContext context, ITemplate template, Request request, Response response)
        {
            foreach (var filter in filterBeforeRander)
            {
                if (!filter(context, template, request, response))
                    return false;
            }
            return true;
        }

        bool ISetting.AfterRanderFilter(HttpContext context, ITemplate template, Request request, Response response)
        {
            foreach (var filter in filterBeforeRander)
            {
                if (!filter(context, template, request, response))
                    return false;
            }
            return true;
        }
    }
}
";

            Regex regliner = new Regex("(\r([^\n]))", RegexOptions.Singleline);
            codetxt = regliner.Replace(codetxt, "\r\n$2");

            Regex regline = new Regex("([^\r])\n", RegexOptions.Singleline);
            codetxt = regline.Replace(codetxt, "$1\r\n");


            return codetxt;
        }
    }
}
