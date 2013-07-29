using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using oyster.web.hosting;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public TemplateBase Template { get; internal set; }
        public RequestHead Head { get; set; }
        public RequestBody Body { get; set; }

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
                Body = body,
                Template = TemplateManager.GetTemplateInstance(typeof(T))
            };
        }

        public void Block<T>(Func<string, dynamic> initBlockAction) where T : TemplateBase
        {

        }
    }
}
