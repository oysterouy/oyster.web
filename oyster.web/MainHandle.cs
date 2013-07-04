using System;
using System.Web;
using System.Text;

namespace oyster.web
{
    public abstract class MainHandle : IHttpHandler
    {
        public MainHandle()
        {
            Init();
        }
        #region IHttpHandler Members

        public bool IsReusable
        {
            // Return false in case your Managed Handler cannot be reused for another request.
            // Usually this would be false in case you have some state information preserved per request.
            get { return true; }
        }
        protected abstract void Init();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ClearHeaders();
            context.Response.Clear();
            context.Response.HeaderEncoding = Encoding.UTF8;


            var templateType = MapTemplate(context);
            if (templateType == null)
            {
                //baseMap
            }
            if (templateType == null)
                throw new Exception("404");

            var t = TemplateFactory.GetTemplateInstance(templateType);
            var set = TemplateFactory.GetTemplateSetting(templateType);

            var info = t.Request();
            if (set != null)
            {
                set.Filter(FilterOnEnum.AfterRoute, context, t, null);
            }


        }

        #endregion

        protected abstract Type MapTemplate(HttpContext context);
    }
}
