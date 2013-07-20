using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using System.Web;

namespace oyster.web
{
    [Serializable]
    public class TemplateApplication
    {
        Dictionary<string, Type> dicTemplates = new Dictionary<string, Type>();
        public void AddTemplate(Type tempType)
        {
            string[] names = tempType.FullName.Split(new char[] { '.' }, StringSplitOptions.RemoveEmptyEntries);
            if (names.Length > 1)
            {
                string nm = "/" + string.Join("/", names, 1, names.Length - 1);
                dicTemplates.Add(nm.ToLower(), tempType);
            }
        }
        public virtual Type MapTemplate(HttpContext context)
        {
            string path = context.Request.Path.ToLower().Trim();
            path = path.EndsWith("/") ? path + "index" : path;
            Type tp = null;
            if (!dicTemplates.TryGetValue(path, out tp))
            {

            }
            return tp;
        }

        public virtual RequestInfo Execute(HttpContext context)
        {
            string nnnn = AppDomain.CurrentDomain.FriendlyName;
            var templateType = MapTemplate(context);
            if (templateType == null)
            {
                //baseMap
            }
            if (templateType == null)
            {
                HttpErrorFactory.Err404(context);
                return null;
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
            return reqInfo;
        }
    }
}
