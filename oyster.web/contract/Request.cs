using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public ITemplate Template { get; internal set; }
        public RequestHead Head { get; set; }
        public dynamic Body { get; set; }

        public Response Execute()
        {
            Response resp = new Response { Template = Template };

            resp.waitHandle = new AutoResetEvent(false);
            ThreadPool.QueueUserWorkItem((state) =>
            {
                try
                {
                    var it = state as ITemplate;
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


            return resp;
        }

        public void Layout<T>(Func<dynamic> initLayoutAction) where T : ITemplate
        {

        }

        public void Block<T>(Func<string, dynamic> initBlockAction) where T : ITemplate
        {

        }
    }
}
