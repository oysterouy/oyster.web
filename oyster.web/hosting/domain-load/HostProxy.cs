using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.hosting
{
    public class HostProxy : MarshalByRefObject
    {
        public string ProxyModuleName { get; set; }
        internal void SetHosting(HostBase set)
        {
            ProxyModuleName = set.GetType().Assembly.FullName;
        }
        internal HostBase GetHosting()
        {
            return TemplateManager.GetSetting(ProxyModuleName);
        }
        public ResponseInfo DoRequest(RequestHead header)
        {
            return GetHosting().DoRequest(header);
        }
    }
}
