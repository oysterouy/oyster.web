using System;
using System.Threading;
using oyster.web.hosting;
using oyster.web.define;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public Request()
        {
            Head = new RequestHead();
            Body = new RequestBody();
        }
        public TemplateBase Template { get; internal set; }
        public RequestHead Head { get; internal set; }
        public RequestBody Body { get; internal set; }

        public Response Execute()
        {
            Response resp = new Response { Template = Template };
            if (LayoutRequest != null)
            {
                resp.LayoutResponse = LayoutRequest.Execute();
            }
            //todo:check cache
            ResponseInfo respInfo = null;//get from cache;
            if (respInfo != null)
            {
                resp.Head = respInfo.Header;
                resp.Body = respInfo.Body;
            }
            else
            {
                resp.waitHandle = new AutoResetEvent(false);
                ThreadPool.QueueUserWorkItem((state) =>
                {
                    try
                    {
                        var it = state as TemplateBase;
                        it.Request(this, resp);
                    }
                    catch (Exception ex)
                    {
                        resp.Exception = ex;
                    }
                    finally
                    {
                        resp.waitHandle.Set();
                    }
                }, Template);
            }

            return resp;
        }

        public Request LayoutRequest { get; set; }
        public void Layout<T>(RequestBody body = null) where T : TemplateBase
        {
            LayoutRequest = new Request
            {
                Head = Head,
                Template = TemplateManager.GetTemplateInstance(typeof(T))
            };
            if (body == null)
            {
                var parms = LayoutRequest.Template.Init(LayoutRequest);
                LayoutRequest.Body.Paramters = parms;
            }

        }

        public void Block<T>(Func<string, DynamicModel> initBlockAction) where T : TemplateBase
        {

        }
    }
}
