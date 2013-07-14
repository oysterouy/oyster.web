using System;
using System.Web;
using System.Text;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;

namespace oyster.web
{
    public abstract class MainHandle : IHttpHandler
    {
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
        protected Dictionary<AppDomainDTO, AppDomain> ApplicationDic { get; set; }
        protected abstract AppDomainDTO MapApplication(HttpContext context);
        public void ProcessRequest(HttpContext context)
        {
            ProcessRequestInner(context);
        }

        protected virtual void ProcessRequestInner(HttpContext context)
        {
            try
            {
                context.Response.ClearHeaders();
                context.Response.Clear();
                context.Response.HeaderEncoding = Encoding.UTF8;

                var app = MapApplication(context);
                if (app == null)
                {
                    HttpErrorFactory.Err404(context);
                    return;
                }
                CallContext.LogicalSetData("HttpContext", context);
                ApplicationDic[app].DoCallBack(app.Execute);
            }
            finally
            {
                context.Response.Flush();
                context.Response.End();
            }
        }

        #endregion
    }
}
