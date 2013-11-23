using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

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
            @"D:\oyster\project\oyster.web\Demo\Demo.Theme\",
            ".cshtml",
            "DemoTheme",
            @"D:\oyster\project\oyster.web\Demo\Demo.Theme\Demo.Theme.csproj"
            } : args;


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
                    string fnameSpace = f.Substring(dir.Length, f.Length - dir.Length - ext.Length)
                        .Replace(System.IO.Path.DirectorySeparatorChar, '.')
                        .Replace(' ', '_');
                    fnameSpace = fnameSpace.StartsWith(".") ? fnameSpace.Substring(1) : fnameSpace;

                    fnameSpace = nameSpace + "." + fnameSpace;

                    string txt = System.IO.File.ReadAllText(f);
                    string code = CodeGenerator.MakeCode(txt, fnameSpace, f);

                    string codef = f + ".cs";

                    System.IO.File.WriteAllText(codef, code, Encoding.UTF8);

                    lsGenCodeFiles.Add(codef);
                }

                if (lsGenCodeFiles.Count > 0 && args.Length > 3 && !string.IsNullOrEmpty(args[3]))
                {
                    string csproj = args[3];
                    if (!System.IO.File.Exists(csproj))
                    {
                        throw new Exception(string.Format("项目文件{0}地址有误.", csproj));
                    }

                    var doc = new XmlDocument();
                    doc.Load(csproj);
                    if (doc.ChildNodes.Count < 2)
                        throw new Exception(string.Format("项目文件{0}内容有误.", csproj));

                    var projectNode = doc.ChildNodes[1];

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
                    bool changed = false;
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
                        changed = true;
                    }
                    if (changed)
                        doc.Save(csproj);
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("未知错误位置 : error : " + ex.ToString());
                Environment.ExitCode = 32;
            }
        }
    }
}
