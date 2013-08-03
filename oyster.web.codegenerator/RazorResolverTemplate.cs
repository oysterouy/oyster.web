﻿using System;
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
            string paramsMethod = null, initMethod = null, requestMethod = null, pstr = "";

            var keyLs = r.OutCodeList.Keys.ToArray();
            foreach (var codeIdx in keyLs)
            {
                string code = r.OutCodeList[codeIdx];
                if (code.Contains("TemplateHelper.Init("))
                {
                    var m = regRequest.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        if (initMethod == null)
                        {
                            string[] parms = m.Groups[1].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            string parmName = "";
                            if (parms.Length > 1)
                                parmName = m.Groups[1].Value;
                            else
                                parmName = "Request " + parms[0];


                            initMethod = string.Format(@"
        public override object[] Init({0})
        {1}
            {2}
        {3}", new string[] { parmName, "{", m.Groups[2].Value, "}" });


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
                        if (requestMethod == null)
                        {
                            string[] ps = m.Groups[1].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                            if (ps.Length < 2)
                            {
                                Console.WriteLine(_fileFullPath + ": error :TemplateHelper.Request(request,response) 参数设置不正确!");
                                Environment.Exit(1);
                            }
                            requestMethod = string.Format(@"
        public dynamic RequestInternal({0})
        {2}
            {1}
        {3}
", new string[] { m.Groups[1].Value, m.Groups[2].Value, "{", "}" });

                            string pVal = "";
                            for (int i = 0; i < ps.Length - 1; i++)
                            {
                                string pType = ps[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                                pVal += string.Format("({0})parms[{1}],", pType, i);
                            }

                            string requestOverride = @"
        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            var model= RequestInternal(" + pVal + @"response);
            if (response.Model != model)
                throw new Exception(""Please Set Model To Response.Model!"");
        }
";
                            requestMethod += requestOverride;

                        }
                    }
                    r.OutCodeList[codeIdx] = null;
                }
                else if (code.Contains("TemplateHelper.Block("))
                {
                    r.OutCodeList[codeIdx] = null;
                }
            }
            StringBuilder codeBody = new StringBuilder();
            for (int i = 0; i < r.NodeCount; i++)
            {
                var section = new KeyValuePair<string, RazorResolver>();
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
                else if (r.sectionCodeList.TryGetValue(i, out section))
                {
                    codeBody.AppendFormat("\r\n            invorker.Invork(typeof({0}),\"{1}\");", ClassName, section.Key);
                }
            }
            Regex regxModel = new Regex("dynamic\\s*Model\\s*=\\s*null;");
            string codeBodyText = regxModel.Replace(codeBody.ToString(), "");

            StringBuilder codeSection = new StringBuilder();
            codeSection.AppendFormat("templateSections.Add(\"{0}\",(html,response,invorker)=>{1});\r\n", "Page",
                    @"{
    dynamic Model=response.Model;
" + codeBodyText + "}");


            foreach (var idx in r.sectionCodeList.Keys)
            {
                var kv = r.sectionCodeList[idx];
                var secR = kv.Value;
                var secFunc = new StringBuilder();
                for (int i = 0; i < secR.NodeCount; i++)
                {
                    string code = "";
                    if (secR.CodeList.TryGetValue(i, out code))
                    {
                        if (string.IsNullOrEmpty(code))
                            continue;
                        secFunc.Append(code);
                    }
                    else if (secR.OutCodeList.TryGetValue(i, out code))
                    {
                        if (string.IsNullOrEmpty(code))
                            continue;
                        secFunc.AppendFormat("\r\n            Echo(html, {0});", code);
                    }
                    else if (secR.StaticHtmlList.TryGetValue(i, out code) && code.Trim().Length > 0)
                    {
                        if (string.IsNullOrEmpty(code))
                            continue;
                        secFunc.AppendFormat("\r\n            Echo(html, @\"{0}\");", code.Replace("\"", "\"\""));
                    }
                }
                codeSection.AppendFormat("templateSections.Add(\"{0}\",(html,response,invorker)=>{1});\r\n", kv.Key,
                    @"{
    dynamic Model=response.Model;
" + secFunc.ToString() + "}");

            }

            string codeUsing = "";
            r.UsingNames.Add("oyster.web.define");
            r.UsingNames.Sort();
            foreach (string s in r.UsingNames)
            {
                codeUsing += string.Format("    using {0};\r\n", s);
            }


            string codetxt = @"
namespace " + NameSpace + @"
{
    using oyster.web;
" + codeUsing + @"
    public class " + ClassName + @" : TemplateBase<" + ClassName + @">
    {
        static " + ClassName + @"()
        {
            " + codeSection.ToString() + @"
        }

        " + initMethod + @"
        " + requestMethod + @"
    }
}
";


            /* ------old --------
            string codetxt = @"
namespace " + NameSpace + @"
{
    using oyster.web;
" + codeUsing + @"
    public class " + ClassName + @" : TemplateBase
    {
        static " + ClassName + @"()
        {
            " + codeSection.ToString() + @"
        }
" + paramsMethod
  + @"

        dynamic ITemplate.Init(Request request)
        {
            return Init(request);
        }
"
  + ireqMethod + initMethod + @"
        void ITemplate.Request(Request request,Response response)
        {
            Request(request,response);
        }
" + requestMethod + @"

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
            */
            Regex regliner = new Regex("(\r([^\n]))", RegexOptions.Singleline);
            codetxt = regliner.Replace(codetxt, "\r\n$2");

            Regex regline = new Regex("([^\r])\n", RegexOptions.Singleline);
            codetxt = regline.Replace(codetxt, "$1\r\n");


            return codetxt;
        }
    }
}
