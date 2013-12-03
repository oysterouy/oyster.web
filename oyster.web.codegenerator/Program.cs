using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Configuration;

namespace oyster.web.codegenerator
{
    class Program
    {
        /// <summary>
        /// 代码生成器
        /// </summary>
        /// <param name="args">
        /// 接收4个参数
        /// 1：模板目录
        /// 2：模板扩展名
        /// 3：顶命名空间
        /// 4:项目文件.csproj地址
        /// </param>
        static void Main(string[] args)
        {
            try
            {
                args = args.Length < 1 ? new string[] {
            @"E:\work\develop\projects\gelu.business.main\deps\common\oyster.web\Demo\TimSiteDemo",
            ".cshtml",
            "TimSiteDemo",
            @"E:\work\develop\projects\gelu.business.main\deps\common\oyster.web\Demo\TimSiteDemo\TimSiteDemo.csproj"
            } : args;

                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RazorProjectGuid"]))
                {
                    Config.RazorProjectGuid = ConfigurationManager.AppSettings["RazorProjectGuid"];
                }
                if (!string.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["RazorConfigType"]))
                {
                    Config.RazorConfigType = ConfigurationManager.AppSettings["RazorConfigType"];
                }

                if (args == null || args.Length < 1 || string.IsNullOrEmpty(args[0]))
                    throw new Exception("请设置模板所在的顶目录.");

                string dir = args[0];
                if (!System.IO.Directory.Exists(dir))
                {
                    throw new Exception("模板顶目录设置有误.");
                }
                string ext = ".cshtml";
                if (args.Length > 1 && !string.IsNullOrEmpty(args[1]))
                {
                    if (!args[1].StartsWith("."))
                        throw new Exception("模板扩展名必须以'.'开头.");

                    ext = args[1];
                }
                string nameSpace = "oyster.web.themes";
                if (args.Length > 2 && !string.IsNullOrEmpty(args[2]))
                    nameSpace = args[2].ToLower().Replace('-', '_');

                var lsGenCodeFiles = new List<string>();
                var fs = System.IO.Directory.GetFiles(dir, "*" + ext, SearchOption.AllDirectories);
                foreach (string f in fs)
                {
                    string classFullName = f.Substring(dir.Length, f.Length - dir.Length - ext.Length)
                        .Replace(System.IO.Path.DirectorySeparatorChar, '.')
                        .Replace(' ', '_');
                    classFullName = classFullName.StartsWith(".") ? classFullName.Substring(1) : classFullName;

                    classFullName = nameSpace + "." + classFullName;

                    string txt = System.IO.File.ReadAllText(f);
                    string code = CodeGenerator.MakeCode(txt, classFullName, f);

                    string codef = f + ".cs";

                    System.IO.File.WriteAllText(codef, code, Encoding.UTF8);

                    lsGenCodeFiles.Add(codef);
                }

                if (args.Length > 3 && !string.IsNullOrEmpty(args[3]))
                {
                    string csproj = args[3];
                    if (!System.IO.File.Exists(csproj))
                    {
                        throw new Exception(string.Format("项目文件{0}地址有误.", csproj));
                    }

                    var doc = new XmlDocument();
                    doc.Load(csproj);
                    StringWriter oldsw = new StringWriter();
                    doc.Save(oldsw);
                    string oldXmlProj = oldsw.ToString();

                    if (doc.ChildNodes.Count < 2)
                        throw new Exception(string.Format("项目文件{0}内容有误.", csproj));

                    var projectNode = doc.ChildNodes[1];

                    XmlNode projectGroupNode = null;
                    foreach (XmlNode nd in projectNode.ChildNodes)
                    {
                        if (projectGroupNode != null)
                            break;
                        if (nd.Name != "PropertyGroup")
                            continue;

                        bool hadGet = false;
                        foreach (XmlNode cnd in nd.ChildNodes)
                        {
                            if (cnd.Name == "ProjectTypeGuids")
                            {
                                if (!cnd.InnerText.Contains(Config.RazorProjectGuid))
                                {
                                    cnd.InnerText = string.Format("{0};{1}", Config.RazorProjectGuid, cnd.InnerText);
                                    hadGet = true;
                                }
                                break;
                            }
                        }
                        if (hadGet)
                            break;
                    }

                    XmlNode pNode = null;
                    foreach (XmlNode nd in projectNode.ChildNodes)
                    {
                        if (pNode != null)
                            break;
                        if (nd.Name != "ItemGroup")
                            continue;
                        foreach (XmlNode cnd in nd.ChildNodes)
                        {
                            if (cnd.Name == "Compile")
                            {
                                pNode = nd;
                                break;
                            }
                        }
                    }

                    List<string> hadFiles = new List<string>();
                    foreach (XmlNode nd in pNode.ChildNodes)
                    {
                        if (nd.Name != "Compile")
                            continue;

                        if (nd.Attributes["Include"] == null)
                            continue;

                        hadFiles.Add(nd.Attributes["Include"].Value);
                    }

                    string dirproj = Path.GetDirectoryName(csproj);

                    foreach (string f in lsGenCodeFiles)
                    {
                        string fname = f.Substring(dirproj.Length + 1);
                        if (hadFiles.Contains(fname))
                            continue;

                        var cmpNode = doc.CreateElement("Compile", projectNode.NamespaceURI);
                        var attr = doc.CreateAttribute("Include");
                        attr.Value = fname;

                        cmpNode.Attributes.Append(attr);

                        var depNode = doc.CreateElement("DependentUpon", projectNode.NamespaceURI);
                        depNode.InnerText = Path.GetFileNameWithoutExtension(fname);

                        cmpNode.AppendChild(depNode);
                        pNode.AppendChild(cmpNode);
                    }

                    foreach (XmlNode nd in projectNode.ChildNodes)
                    {
                        if (nd.Name != "ItemGroup")
                            continue;
                        foreach (XmlNode cnd in nd.ChildNodes)
                        {
                            if (cnd.Name == "Reference"
                                && cnd.Attributes["Include"] != null
                                &&
                                (cnd.Attributes["Include"].Value.Contains("System.Web.WebPages.Deployment,")
                                ||
                                cnd.Attributes["Include"].Value.Contains("System.Web.WebPages.Razor,"))
                                )
                            {
                                bool hadSet = false;
                                foreach (XmlNode priNode in cnd.ChildNodes)
                                {
                                    if (priNode.Name == "Private")
                                    {
                                        priNode.InnerText = "True";
                                        hadSet = true;
                                    }
                                }
                                if (!hadSet)
                                {
                                    var node = doc.CreateElement("Private", projectNode.NamespaceURI);
                                    node.InnerText = "True";
                                    cnd.AppendChild(node);
                                }
                            }
                        }
                    }


                    StringWriter sw = new StringWriter();
                    doc.Save(sw);
                    if (!sw.ToString().Equals(oldXmlProj))
                    {
                        //changed
                        doc.Save(csproj);
                    }

                    ModifyWebConfig(Path.Combine(Path.GetDirectoryName(csproj), "Web.config"));
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("未知错误位置 : error : " + ex.ToString());
                Environment.ExitCode = 32;
            }
        }

        static void ModifyWebConfig(string path)
        {
            if (!File.Exists(path)) return;
            var doc = new XmlDocument();
            doc.Load(path);
            StringWriter oldsw = new StringWriter();
            doc.Save(oldsw);
            string oldWebConfig = oldsw.ToString();

            XmlNode configurationNode = null;
            foreach (XmlNode nd in doc.ChildNodes)
            {
                if (nd.Name == "configuration")
                {
                    configurationNode = nd;
                    break;
                }
            }

            XmlNode configSections = null;
            foreach (XmlNode nd in configurationNode.ChildNodes)
            {
                if (nd.Name == "configSections")
                {
                    configSections = nd;
                    break;
                }
            }
            if (configSections == null)
            {
                configSections = doc.CreateElement("configSections");
                configurationNode.InsertBefore(configSections, configurationNode.FirstChild);
            }
            bool hadRazorSection = false;
            foreach (XmlNode nd in configSections.ChildNodes)
            {
                if (nd.Name == "sectionGroup" && nd.Attributes["name"] != null
                    && nd.Attributes["name"].Value == "system.web.webPages.razor")
                {
                    hadRazorSection = true;
                    break;
                }
            }

            if (!hadRazorSection)
            {
                var sectionGroup = doc.CreateElement("sectionGroup");
                var attrName = doc.CreateAttribute("name");
                attrName.Value = "system.web.webPages.razor";
                sectionGroup.Attributes.Append(attrName);

                var attrType = doc.CreateAttribute("type");
                attrType.Value = Config.RazorConfigType;
                sectionGroup.Attributes.Append(attrType);

                configSections.InsertBefore(sectionGroup, configSections.FirstChild);

                sectionGroup.InnerXml = @"<section name=""pages"" type=""" + Config.RazorPagesConfigType + @""" requirePermission=""false"" />";


                var razorNode = doc.CreateElement("system.web.webPages.razor");
                razorNode.InnerXml =
@"<pages pageBaseType=""oyster.web.TimTemplateBase,oyster.web"">
      <namespaces>
        <add namespace=""System"" />
        <add namespace=""System.Linq"" />
        <add namespace=""System.Text"" />
        <add namespace=""System.Web""/>
        <add namespace=""oyster.web"" />
        <add namespace=""oyster.web.define"" />
      </namespaces>
    </pages>";
                configurationNode.InsertAfter(razorNode, configSections);

                //var sectionNode = doc.CreateElement("section");
                //var attrSecName = doc.CreateAttribute("name");
                //attrSecName.Value = "pages";
                //sectionNode.Attributes.Append(attrSecName);

                //var attrSecType = doc.CreateAttribute("type");
                //attrSecType.Value = "System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35";
                //sectionNode.Attributes.Append(attrSecType);

                //var attrSecrequirePermission = doc.CreateAttribute("requirePermission");
                //attrSecrequirePermission.Value = "false";
                //sectionNode.Attributes.Append(attrSecrequirePermission);
            }

            StringWriter sw = new StringWriter();
            doc.Save(sw);
            if (!sw.ToString().Equals(oldWebConfig))
            {
                //changed
                doc.Save(path);
            }
        }
    }
}
