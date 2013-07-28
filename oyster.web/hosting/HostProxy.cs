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
        public Response DoRequest(RequestHead header)
        {
            return GetHosting().DoRequest(header);
        }
        public ITemplate Route(Request request)
        {
            return GetHosting().Route(request);
        }

        public bool BeforeRouteFilter(Request request)
        {
            return GetHosting().BeforeRouteFilter(request);
        }

        public bool BeforeRequestFilter(Request request)
        {
            return GetHosting().BeforeRequestFilter(request);
        }

        public bool BeforeRanderFilter(Request request, Response response)
        {
            return GetHosting().BeforeRanderFilter(request, response);
        }

        public bool AfterRanderFilter(Request request, Response response)
        {
            return GetHosting().BeforeRanderFilter(request, response);
        }

        public int LoadingTimeout
        {
            get { return GetHosting().LoadingTimeout; }
        }
    }
}
