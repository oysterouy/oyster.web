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
            Response = new Response();
            BlockTemplates = new KeyValueCollection<string, TimBlock>();

            TimProcessContext.SetProcess(this);
            Theme = Host.GetTheme(Request);
            if (Theme == null)
                throw new Exception("No Theme Match This Request!");
            if (!Theme.BeforeRouteFilter(this))
                return;
            Theme.Init();
        }
        internal TimProcess(TimHost host, Request request, bool isMain)
            : this(host, request)
        {
            IsMainProcess = isMain;
        }
        public TimHost Host { get; protected set; }
        public TimTheme Theme { get; protected set; }
        public Request Request { get; protected set; }
        public Response Response { get; protected set; }
        public TimTemplate Template { get; protected set; }
        public TimProcess Layout { get; protected set; }

        internal KeyValueCollection<string, TimBlock> BlockTemplates { get; set; }
        TimBlock MainBlock { get; set; }
        internal void InitMainBlock()
        {
            MainBlock = new TimBlock
                {
                    CallID = Guid.NewGuid().ToString(),
                    Template = Template,
                    Parameters = Response.Paramters,
                    Process = this,
                };
        }
        internal void InvokeMainBlock()
        {
            MainBlock.Invoke();
        }

        bool IsMainProcess { get; set; }
        internal bool IsMainLayoutProcess { get { return IsMainProcess && Layout == null; } }
        internal int LoadingTimeout
        {
            get
            {
                if (IsMainLayoutProcess || Request.LoadingTimeout < 0 || Theme.LoadingTimeout < 0)
                    return 0;

                if (Request.LoadingTimeout > 0)
                    return Request.LoadingTimeout;

                else if (Theme.LoadingTimeout > 0)
                    return Theme.LoadingTimeout;

                return 500;
            }
        }

        public bool IsError { get { return ErrorResponse != null; } }
        public Response ErrorResponse { get; set; }

        public TimProcess Process()
        {
            Template = Theme.Route.Match(Request);
            if (Template == null)
            {
                Response = ResponseManager.Instance.GetResponseByStatusCode(404);
                return this;
            }
            Template.Init(this);

            MainBlock.Invoke().Render();

            return this;
        }

        public Response OutputResponse
        {
            get
            {
                if (Layout != null)
                    return Layout.OutputResponse;
                return Response;
            }
        }

        internal void ProcessRequest()
        {
            TimProcessContext.SetProcess(this);
            if (!Theme.BeforeRequestFilter(this))
                return;
            if (Layout != null)
                Layout.InvokeMainBlock();
            Template.Request(this);
        }

        internal void ProcessRender()
        {
            TimProcessContext.SetProcess(this);
            if (!Theme.BeforeRenderFilter(this))
                return;
            Template.Render(this);
            if (!Theme.AfterRenderFilter(this))
                return;
        }

        internal bool SetLayout(TimTemplate layout, params object[] args)
        {
            Layout = new TimProcess(Host, Request, IsMainProcess);
            Layout.Template = layout;
            layout.Init(Layout, args);
            return !Layout.IsError;
        }
        internal string BlockRender(TimTemplate block, string callID)
        {
            TimBlock bl = null;
            if (BlockTemplates.TryGetValue(callID, out bl))
            {
                bl.Render();
            }
            throw new Exception(string.Format("Block type:{0},callId:{1} have no Invoke Before!",
                block.GetType().FullName, callID));
        }
        internal void BlockInvoke(TimTemplate block, string callID, params object[] args)
        {
            if (BlockTemplates.ContainsKey(callID))
                throw new Exception(string.Format("CallID:{0} Had Be Used!", callID));
            var bl = new TimBlock { CallID = callID, Template = block, Parameters = args };
            bl.Process = new TimProcess(Host, Request);
            BlockTemplates.Add(callID, bl);
            bl.Invoke();
        }
    }
}
