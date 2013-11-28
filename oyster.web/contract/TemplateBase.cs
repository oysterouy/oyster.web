using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using oyster.web.define;
using System.IO;


namespace oyster.web
{
    public abstract class TemplateBase : TimTemplateBase
    {
        internal abstract Dictionary<string, Action<StringBuilder, Response, SectionInvork>> Sections { get; }

        protected static StringBuilder Echo(StringBuilder html, object p)
        {
            html.Append(p == null ? "" : p.ToString());
            return html;
        }
        public abstract object[] Init(Request request);
        public abstract void Request(Request request, Response response);
    }

    public abstract class TemplateBase<T> : TemplateBase where T : TemplateBase
    {
        protected static readonly Dictionary<string, Action<StringBuilder, Response, SectionInvork>> templateSections = new Dictionary<string, Action<StringBuilder, Response, SectionInvork>>();

        internal override Dictionary<string, Action<StringBuilder, Response, SectionInvork>> Sections
        {
            get { return templateSections; }
        }
    }
}
