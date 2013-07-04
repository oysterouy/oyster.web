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

        }

        protected override Type MapTemplate(HttpContext context)
        {
            throw new NotImplementedException();
        }
    }
}