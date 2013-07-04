using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Host
{
    public class TemplateFactory : oyster.web.TemplateFactory
    {
        public override oyster.web.ITemplate MapTemplate(HttpContext context)
        {
            var iset = new demotheme.Settings() as oyster.web.ISetting;
            var t = iset.Route(context);
            if (t == null)
            {
                //base template
            }
            return t;
        }
    }
}