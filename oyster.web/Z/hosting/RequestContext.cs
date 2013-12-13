using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace oyster.web.define
{
    class RequestContext
    {
        static string GetHostContextKey()
        {
            return string.Format("call-context:{0}", typeof(HostBase).FullName);
        }
        public static void SetHost(HostBase host)
        {
            CallContext.SetData(GetHostContextKey(), host);
        }
        public static HostBase GetHost()
        {
            var obj = CallContext.GetData(GetHostContextKey());
            return obj as HostBase;
        }
    }
}
