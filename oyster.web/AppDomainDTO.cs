using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Reflection;
using System.IO;
using System.Runtime.Remoting.Messaging;

namespace oyster.web
{
    public class AppDomainDTO : MarshalByRefObject
    {
        public string DllPath { get; set; }

        public void Load()
        {
            string nnnn = AppDomain.CurrentDomain.FriendlyName;
            var bs = File.ReadAllBytes(DllPath);
            var asm = Assembly.Load(bs);
            var tps = asm.GetTypes();
            var ls = new List<Type>();
            foreach (var tp in tps)
            {
                var itface = tp.GetInterface(typeof(ITemplate).FullName);
                if (itface != null)
                {
                    ls.Add(tp);
                }
            }

            Application = new TemplateApplication();
            foreach (var tp in ls)
            {
                Application.AddTemplate(tp);
            }
        }

        public TemplateApplication Application { get; set; }

        public void Execute()
        {
            string nnnn = AppDomain.CurrentDomain.FriendlyName;
            var context = (HttpContext)CallContext.LogicalGetData("HttpContext");
            Application.Execute(context);
        }
    }
}
