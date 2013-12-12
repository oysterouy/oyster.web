using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace oyster.web.A.implement
{
    class TimProcessContext
    {
        static string GetProcessContextKey()
        {
            return string.Format("call-context:{0}", typeof(TimProcess).FullName);
        }
        public static void SetProcess(TimProcess host)
        {
            CallContext.SetData(GetProcessContextKey(), host);
        }
        public static TimProcess GetProcess()
        {
            var obj = CallContext.GetData(GetProcessContextKey());
            return obj as TimProcess;
        }
    }
}
