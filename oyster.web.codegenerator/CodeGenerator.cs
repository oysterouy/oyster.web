using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace oyster.web.codegenerator
{
    interface IResolve
    {
        string DoResolve();
    }
    class CodeGenerator
    {
        public static string MakeCode(string templateCode, string classFullName, string filePath)
        {
            if (classFullName.ToLower().EndsWith("settings"))
            {
                return new RazorResolverSettings(templateCode, classFullName, filePath).DoResolve();
            }
            else
            {
                var rt = new RazorResolverTemplate(templateCode, classFullName, filePath);
                return rt.DoResolve();
            }
        }
    }
}
