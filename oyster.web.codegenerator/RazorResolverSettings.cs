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

            Regex regFilter = new Regex("TemplateHelper\\.Filter\\(([^,]+),(.*)\\)", RegexOptions.Singleline);

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
                else if (code.Contains("TemplateHelper.Filter("))
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
                string lsName = "ls" + i++;
                addcodeFilter.AppendFormat(@"
            var " + lsName + @" = new List<Func<HttpContext, ITemplate, StringBuilder, bool>>();
            filterDic.Add({0}, " + lsName + @");
", fon);
                foreach (var f in ls)
                {
                    addcodeFilter.AppendFormat("            " + lsName + ".Add({0});\r\n", f);
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

        static readonly Dictionary<FilterOnEnum,
            List<Func<HttpContext, ITemplate, StringBuilder, bool>>> filterDic =
            new Dictionary<FilterOnEnum, List<Func<HttpContext, ITemplate, StringBuilder, bool>>>();

        static Settings()
        {
            //******** route setting *********//
 " + addcodeRoute + @"

            //******** filter setting *********//
" + addcodeFilter + @"
        }

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

        bool ISetting.Filter(FilterOnEnum fon, HttpContext context, ITemplate it, StringBuilder str)
        {
            List<Func<HttpContext, ITemplate, StringBuilder, bool>> ls = null;
            if (filterDic.TryGetValue(fon, out ls))
            {
                foreach (var f in ls)
                {
                    if (!f(context, it, str))
                        return false;
                }
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
