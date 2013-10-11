using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Compilation;
using System.CodeDom;
using System.IO;

namespace oyster.web.complation
{
    public class ClearCodeBuildProvider : BuildProvider
    {
        public override void GenerateCode(AssemblyBuilder assemblyBuilder)
        {
            //just let cshtml template not be use Razor BuildProvider.
            //clear code like Html,Request,and so on.
        }
    }
}
