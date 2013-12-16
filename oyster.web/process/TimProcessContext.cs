using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Remoting.Messaging;

namespace oyster.web
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

        public static Response GetResponse()
        {
            var process = GetProcess();
            if (process != null)
                return process.Response;
            return null;
        }
        public static Response GetResponseModel()
        {
            var response = GetResponse();
            if (response == null)
                throw new Exception("ProcessContext don't get Response.");

            return response.Model;
        }
    }
}
