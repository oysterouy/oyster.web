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
        public Request(ITemplate temp)
        {
            Template = temp;
        }
        public ITemplate Template { get; protected set; }
        public RequestHead Head { get; set; }
        public dynamic Body { get; set; }

        public Response Execute()
        {
            Exception exeEx = null;
            Response resp = new Response(Template);

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
    }

    public class Request<T> : Request
        where T : ITemplate, new()
    {
        public Request()
            : base(new T())
        {
        }
    }
}
