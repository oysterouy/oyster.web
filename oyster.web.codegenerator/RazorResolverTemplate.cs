using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace oyster.web.codegenerator
{
    class RazorResolverTemplate : IResolve
    {
        public RazorResolverTemplate(string codeText, string classFullName, string filePath)
        {
            _codeText = codeText;
            _fileFullPath = filePath;

            string nameSpace = System.IO.Path.GetFileNameWithoutExtension(classFullName);
            string className = System.IO.Path.GetExtension(classFullName);
            className = className.StartsWith(".") ? className.Substring(1) : className;
            className = className.Substring(0, 1).ToUpper() + className.Substring(1);

            NameSpace = nameSpace;
            ClassName = className;
        }
        string _codeText;
        string _fileFullPath;
        string NameSpace { get; set; }
        string ClassName { get; set; }

        public string DoResolve()
        {
            Regex regParameters = new Regex("TemplateHelper\\.Parameters\\(\\(([^\\)]+)\\)\\s*=>\\s*(.*)\\s*\\)", RegexOptions.Singleline);

            Regex regRequest = new Regex("TemplateHelper\\.Init\\(\\(([^\\)]+)\\)\\s*=>\\s*\\{(.*)\\}\\s*\\)", RegexOptions.Singleline);

            Regex regLoad = new Regex("TemplateHelper\\.Request\\(\\(([^\\)]+)\\)\\s*=>\\s*\\{(.*)\\}\\s*\\)", RegexOptions.Singleline);

            var r = new RazorResolver(_codeText);
            string paramsMethod = null, reqMethod = null, loadMethod = null, pstr = "";

            var keyLs = r.OutCodeList.Keys.ToArray();
            foreach (var codeIdx in keyLs)
            {
                string code = r.OutCodeList[codeIdx];
                if (code.Contains("TemplateHelper.Parameters("))
                {
                    var m = regParameters.Match(code);
                    if (m.Success && m.Groups.Count > 2)
                    {
                        if (paramsMethod == null)
                        {
                            bool hadKh = m.Groups[2].Value.Trim().StartsWith("{");
                            paramsMethod = string.Format(
@"                                           public static object[] Parameters(HttpContext {0})
        {1}
        {2}
        {3}", new string[] { m.Groups[1].Value, hadKh ? "" : "{", m.Groups[2].Value, hadKh ? "" : "}" }).Trim();
                        }
                        r.OutCodeList[codeIdx] = null;
                    }
                }
                else if (code.Contains("TemplateHelper.Init("))
                {
                    var m = regRequest.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        if (reqMethod == null)
                        {
                            reqMethod = string.Format(@"
        public static Request Init({0})
        {1}
            {2}
        {3}", new string[] { m.Groups[1].Value, "{", m.Groups[2].Value, "}" });


                            string[] argTypes = m.Groups[1].Value.Split(new char[] { ' ', ',' }, StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < argTypes.Length; i += 2)
                            {
                                pstr += string.Format(",({0})parms[{1}]", argTypes[i], i / 2);
                            }
                            pstr = pstr.Length > 0 ? pstr.Substring(1) : "";

                        }
                        r.OutCodeList[codeIdx] = null;
                    }
                }
                else if (code.Contains("TemplateHelper.Request("))
                {
                    var m = regLoad.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        if (loadMethod == null)
                        {
                            string[] ps = m.Groups[1].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (ps.Length < 2)
                            {
                                Console.WriteLine(_fileFullPath + ": error :TemplateHelper.Request(request,response) 参数设置不正确!");
                                Environment.Exit(1);
                            }
                            loadMethod = string.Format(@"
        public static void Request(Request {0},Response {1})
        {3}
            {2}
        {4}
", new string[] { ps[0], ps[1], m.Groups[2].Value, "{", "}" });



                        }
                    }
                    r.OutCodeList[codeIdx] = null;
                }
            }

            string ireqMethod = @"
        public static Request Init(HttpContext context){
            var parms = Parameters(context);
            return Init(" + pstr + @");
        }      
";


            StringBuilder codeBody = new StringBuilder();
            for (int i = 0; i < r.NodeCount; i++)
            {
                string code = "";
                if (r.CodeList.TryGetValue(i, out code))
                {
                    if (string.IsNullOrEmpty(code))
                        continue;
                    codeBody.Append(code);
                }
                else if (r.OutCodeList.TryGetValue(i, out code))
                {
                    if (string.IsNullOrEmpty(code))
                        continue;
                    codeBody.AppendFormat("\r\n            Echo(html, {0});", code);
                }
                else if (r.StaticHtmlList.TryGetValue(i, out code) && code.Trim().Length > 0)
                {
                    if (string.IsNullOrEmpty(code))
                        continue;
                    codeBody.AppendFormat("\r\n            Echo(html, @\"{0}\");", code.Replace("\"", "\"\""));
                }
            }

            string codeUsing = "";
            r.UsingNames.Sort();
            foreach (string s in r.UsingNames)
            {
                codeUsing += string.Format("    using {0};\r\n", s);
            }
            Regex regxModel = new Regex("dynamic\\s*Model\\s*=\\s*null;");
            string codeBodyText = regxModel.Replace(codeBody.ToString(), "");

            string codetxt = @"
namespace " + NameSpace + @"
{
    using oyster.web;
" + codeUsing + @"
    public class " + ClassName + @" : ITemplate
    {
" + paramsMethod
  + @"

        Request ITemplate.Init(HttpContext context)
        {
            return Init(context);
        }
"
  + ireqMethod + reqMethod + @"
        void ITemplate.Request(Request request,Response response)
        {
            Request(request,response);
        }
" + loadMethod + @"

        public static StringBuilder Rander(dynamic Model)
        {
            StringBuilder html = new StringBuilder();"
+ codeBodyText
+ @"
            return html;
        }
        
        StringBuilder ITemplate.Rander(dynamic Model)
        {
            return Rander(Model);
        }

        internal  static  StringBuilder Echo(StringBuilder html, object p)
        {
            html.Append(p == null ? """" : p.ToString());
            return html;
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
