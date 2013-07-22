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
        public static string MakeCode(string templateCode, string filePathWithoutExt, string filePath)
        {
            if (filePathWithoutExt.ToLower().EndsWith("settings"))
            {
                return new RazorResolverSettings(templateCode, filePathWithoutExt, filePath).DoResolve();
            }
            else
            {
                var rt = new RazorResolverTemplate(templateCode, filePathWithoutExt, filePath);
                return rt.DoResolve();
            }
        }
    }
}
