﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace oyster.web.codegenerator
{
    public class RazorResolverTemplate : IResolve
    {
        public RazorResolverTemplate(string codeText, string classFullName, string filePath)
        {
            _codeText = codeText;
            _fileFullPath = filePath;

            string nameSpace = System.IO.Path.GetFileNameWithoutExtension(classFullName);
            string className = System.IO.Path.GetExtension(classFullName);
            className = className.StartsWith(".") ? className.Substring(1) : className;
            className = className.Substring(0, 1).ToUpper() + className.Substring(1);

            NameSpace = nameSpace.ToLower();
            ClassName = className;
        }
        string _codeText;
        string _fileFullPath;
        string NameSpace { get; set; }
        string ClassName { get; set; }

        Regex regBlock = new Regex("TimSetting\\.Block\\(\\(([^\\)]+)\\)\\s*=>\\s*(.*)\\s*\\)", RegexOptions.Singleline);

        Regex regRequest = new Regex("TimSetting\\.Init\\(\\(([^\\)]+)\\)\\s*=>\\s*\\{(.*)\\}\\s*\\)", RegexOptions.Singleline);

        Regex regLoad = new Regex("TimSetting\\.Request\\(\\(([^\\)]+)\\)\\s*=>\\s*\\{(.*)\\}\\s*\\)", RegexOptions.Singleline);

        public string DoResolve()
        {
            var r = new RazorResolver(_codeText);
            string blockInvorks = "", initMethod = null, initParamName = "", requestMethod = "", requestMethodImp = "", pstr = "";


            var keyLs = r.OutCodeList.Keys.ToArray();
            foreach (var codeIdx in keyLs)
            {
                string code = r.OutCodeList[codeIdx];
                if (code.Contains("TimSetting.Init("))
                {
                    var m = regRequest.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        if (initMethod == null)
                        {
                            string[] parms = m.Groups[1].Value.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (parms.Length > 1)
                                initParamName = parms[1];
                            else
                                initParamName = parms[0];

                            initMethod = string.Format(@"
        public override object[] Init(Request {0})
        {1}
            /*inputblocks*/
            {2}
        {3}", new string[] { initParamName, "{", m.Groups[2].Value, "}" });


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
                else if (code.Contains("TimSetting.Request("))
                {
                    var m = regLoad.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        string[] ps = m.Groups[1].Value.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                        if (ps.Length < 1)
                        {
                            Console.WriteLine(_fileFullPath + ": error :TimSetting.Request(request,response) 参数设置不正确!");
                            Environment.Exit(1);
                        }
                        //TimSetting.Request((response)=>{})
                        //兼容不带类型的方式
                        if (ps.Length == 1 && ps[0].Trim().IndexOf(' ') < 0)
                        {
                            ps[0] = "Response " + ps[0];
                        }
                        string paramsStr = string.Join(" , ", ps);
                        requestMethod += string.Format(@"
        public void RequestInternal({0})
        {2}
            {1}
        {3}
", new string[] { paramsStr, m.Groups[2].Value, "{", "}" });

                        string pVal = "", pParameters = "", pParametersVal = "";
                        for (int i = 0; i < ps.Length - 1; i++)
                        {
                            string pType = ps[i].Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
                            pVal += string.Format("({0})parms[{1}],", pType, i);
                            pParameters += string.Format("{0} p{1},", pType, i);
                            pParametersVal += string.Format("p{0},", i);
                        }

                        pParameters = pParameters.EndsWith(",") ? pParameters.Substring(0, pParameters.Length - 1) : pParameters;
                        pParametersVal = pParametersVal.EndsWith(",") ? pParametersVal.Substring(0, pParametersVal.Length - 1) : pParametersVal;
                        if (ps.Length == 1)
                        {
                            requestMethodImp += @"
            if(parms.Length==0)
                RequestInternal(response);";
                        }
                        else
                        {
                            requestMethodImp += string.Format(@"
            if(parms.Length=={0})
                RequestInternal({1}response);", ps.Length - 1, pVal);
                        }

                        /*
                        string requestOverride = @"
        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            RequestInternal(" + pVal + @"response);
        }
        
        public static object[] Parameters(" + pParameters + @")
        {
            return new object[] {" + pParametersVal + @" };
        }
";
                        */
                        requestMethod += @"
        public static object[] Parameters(" + pParameters + @")
        {
            return new object[] {" + pParametersVal + @" };
        }";


                    }
                    r.OutCodeList[codeIdx] = null;
                }
                else if (code.Contains("TimSetting.Block("))
                {
                    blockInvorks += BlockResolve(r, code, codeIdx);
                    /*
                    var m = regBlock.Match(code);
                    if (m.Success && m.Groups.Count > 1)
                    {
                        string[] parms = m.Groups[1].Value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parms.Length != 4)
                        {
                            Console.WriteLine(_fileFullPath + ": error :TimSetting.Block((T template,string \"callId\")=>true) 参数设置不正确,必须使用明确类型的参数方式!");
                            Environment.Exit(1);
                        }
                        string template = parms[0];
                        string id = parms[3];
                        bool sync = m.Groups[2].Value.Contains("false");
                        r.OutCodeList[codeIdx] = string.Format("response.BlockRender<{0}>({1},{2})", template, id, sync ? "true" : "false");
                        blockInvorks += string.Format("__initParamName__.BlockRegister<{0}>({1});\r\n", template, id);
                    }
                    else
                        r.OutCodeList[codeIdx] = null;
                    */
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
                    codeBody.AppendFormat("\r\n            invorker.Invoke(typeof({0}),\"{1}\");", ClassName, section.Key);
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

                        if (code.Contains("TimSetting.Block("))
                        {
                            blockInvorks += BlockResolve(secR, code, i);
                            secR.OutCodeList.TryGetValue(i, out code);
                            if (string.IsNullOrEmpty(code))
                                continue;
                        }
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
            r.UsingNames.Sort();
            foreach (string s in r.UsingNames)
            {
                codeUsing += string.Format("    using {0};\r\n", s);
            }
            blockInvorks = blockInvorks.Replace("__initParamName__", initParamName);
            if (string.IsNullOrEmpty(initMethod))
            {
                Console.WriteLine(_fileFullPath + ": error :TimSetting.Init((request)=>{...}) 方法未设置!");
                Environment.Exit(1);
            }
            initMethod = initMethod.Replace("/*inputblocks*/", blockInvorks);
            string codetxt = @"
namespace " + NameSpace + @"
{
    using oyster.web;
" + codeUsing + @"
    public class " + ClassName + @" : TimTemplate<" + ClassName + @">
    {
        static " + ClassName + @"()
        {
            " + codeSection.ToString() + @"
        }

        " + initMethod + @"
        " + requestMethod + @"
        public override void Request(TimProcess timProcess)
        {
            var response = timProcess.Response;
            object[] parms = response.Paramters;
            if(parms==null)
                throw new Exception(""Paramters no set!"");
            " + requestMethodImp + @"        
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

        string BlockResolve(RazorResolver r, string code, int codeIdx)
        {
            string blockInvork = "";
            var m = regBlock.Match(code);
            if (m.Success && m.Groups.Count > 1)
            {
                string[] parms = m.Groups[1].Value.Split(new char[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (parms.Length != 4)
                {
                    Console.WriteLine(_fileFullPath + ": error :TimSetting.Block((T template,string \"callId\")=>true) 参数设置不正确,必须使用明确类型的参数方式!");
                    Environment.Exit(1);
                }
                string template = parms[0];
                string id = parms[3];
                bool sync = m.Groups[2].Value.Contains("false");
                r.OutCodeList[codeIdx] = string.Format("response.BlockRender<{0}>({1},{2})", template, id, sync ? "true" : "false");
                blockInvork = string.Format("__initParamName__.BlockRegister<{0}>({1});\r\n", template, id);
            }
            else
                r.OutCodeList[codeIdx] = null;
            return blockInvork;
        }
    }
}
