using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Host
{
    public class MainHandle : oyster.web.MainHandle
    {
        protected override void Init()
        {
            AddTemplate(typeof(demotheme.Index));
            AddTemplate(typeof(demotheme.Login));
        }
    }
}