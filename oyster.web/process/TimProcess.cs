using oyster.web.host;
using oyster.web.manage;
using System;

namespace oyster.web
{
    public class TimProcess
    {
        public TimProcess(TimHost host, Request request)
        {
            Host = host;
            Request = request;
        }
        public TimHost Host { get; protected set; }
        public TimTheme Theme { get; protected set; }
        public Request Request { get; protected set; }
        public Response Response { get; protected set; }
        public TimTemplate Template { get; protected set; }
        public void Process()
        {
            TimProcessContext.SetProcess(this);
            Theme = Host.GetTheme(Request);
            if (Theme == null)
                throw new Exception("No Theme Match This Request!");
            if (!Theme.BeforeRouteFilter(this))
                return;
            Template = Theme.Route.Match(Request);
            if (Template == null)
            {
                Response = ResponseManager.Instance.GetResponseByStatusCode(404);
                return;
            }
            Template.Init(this);
            if (!Theme.BeforeRequestFilter(this))
                return;
            Template.Request(this);
            if (!Theme.BeforeRanderFilter(this))
                return;
            Template.Rander(this);
            if (!Theme.AfterRanderFilter(this))
                return;
        }
    }
}
