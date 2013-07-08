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
            try
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
                {
                    HttpErrorFactory.Err404(context);
                    return;
                }
                var t = TemplateFactory.GetTemplateInstance(templateType);
                var set = TemplateFactory.GetTemplateSetting(templateType);

                var reqInfo = t.RequestTemplate();
                if (set != null)
                {
                    set.Filter(FilterOnEnum.BeforeRequest, context, t, null);
                }
                reqInfo.Load();
                if (set != null)
                {
                    set.Filter(FilterOnEnum.BeforeLoad, context, t, null);
                }
                var html = reqInfo.Show();
                if (set != null)
                {
                    set.Filter(FilterOnEnum.BeforeShow, context, t, html);
                }
                context.Response.Write(html.ToString());
            }
            finally
            {
                context.Response.Flush();
                context.Response.End();
            }
        }

        #endregion

        protected abstract Type MapTemplate(HttpContext context);
    }
}
