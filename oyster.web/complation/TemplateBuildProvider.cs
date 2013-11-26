using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.CodeDom;
using System.IO;
using System.Reflection;

namespace oyster.web.complation
{
    public class ClearCodeBuildProvider : BuildProvider
    {
        public override void GenerateCode(AssemblyBuilder assemblyBuilder)
        {
            //just let cshtml template not be use Razor BuildProvider.
            //clear code like Html,Request,and so on.

//            string typeNamespace = "AAAA";
//            string typeName = "XXXX";
//            // 创建CodeCompileUnit以包含代码
//            CodeCompileUnit ccu = new CodeCompileUnit(); // 分配需要的命名空间 

//            var assmLs = new List<string> { 
//            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\Microsoft.CSharp.dll",
//            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Core.dll",
//            @"C:\Program Files (x86)\Reference Assemblies\Microsoft\Framework\.NETFramework\v4.0\System.Web.dll",
//            @"D:\oyster\git\develop\oyster.web\oyster.web\bin\debug\oyster.web.dll",
//            };
//            foreach (var asmPath in assmLs)
//            {
//                var assem = Assembly.LoadFrom(asmPath);
//                assemblyBuilder.AddAssemblyReference(assem);
//                ccu.ReferencedAssemblies.Add(assem.FullName);
//            }
//            CodeNamespace cns = new CodeNamespace(typeNamespace);
//            cns.Imports.Add(new CodeNamespaceImport("System"));
//            cns.Imports.Add(new CodeNamespaceImport("System.Text"));
//            cns.Imports.Add(new CodeNamespaceImport("System.Web"));
//            cns.Imports.Add(new CodeNamespaceImport("System.Linq"));
//            cns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
//            cns.Imports.Add(new CodeNamespaceImport("oyster.web"));

//            ccu.Namespaces.Add(cns); // 创建新的类声明
//            CodeTypeDeclaration parentClass = new CodeTypeDeclaration(typeName);
//            cns.Types.Add(parentClass); // 创建获得一个参数并返回一个字符串的SayHello方法
//            CodeMemberMethod method = new CodeMemberMethod();
//            method.Name = "SayHello";
//            method.Attributes = MemberAttributes.Public;
//            CodeParameterDeclarationExpression arg = new CodeParameterDeclarationExpression(typeof(string), "inputMessage");
//            method.Parameters.Add(arg);
//            method.ReturnType = new CodeTypeReference(typeof(string)); // 添加方法实体需要的代码

//            string scriptBody = @"
//            return inputMessage+""dddddd"";
//            ";
//            CodeSnippetStatement methodBody = new CodeSnippetStatement(scriptBody);
//            method.Statements.Add(methodBody);
//            parentClass.Members.Add(method);

//            assemblyBuilder.AddCodeCompileUnit(this, ccu);
        }
    }
}
