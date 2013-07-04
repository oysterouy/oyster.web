using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Host
{
    public class MainHandle : oyster.web.MainHandle
    {
        protected override void Init()
        {
            oyster.web.TemplateFactory.SetFactory(new TemplateFactory());
        }
    }
}