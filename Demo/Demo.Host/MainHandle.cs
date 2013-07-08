using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Host
{
    public class MainHandle : oyster.web.MainHandle
    {
        static Dictionary<string, Type> dicTemplates = new Dictionary<string, Type>();
        void AddTemplate(Type tempType)
        {
            string[] names = tempType.FullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length > 1)
            {
                string nm = string.Join("/", names, 1, names.Length - 1);
                dicTemplates.Add(nm.ToLower(), tempType);
            }
        }

        protected override void Init()
        {
            AddTemplate(typeof(demotheme.Index));
            AddTemplate(typeof(demotheme.Login));
        }

        protected override Type MapTemplate(HttpContext context)
        {
            string path = context.Request.Path.ToLower().Trim();
            path = path.Length > 1 ? path.Substring(1) : "index";
            Type tp = null;
            if (!dicTemplates.TryGetValue(path, out tp))
            {

            }
            return tp;
        }
    }
}