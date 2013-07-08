using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    [Serializable]
    public abstract class RequestInfo
    {
        public abstract Type TemplateType { get; }
        public CacheTypeEnum CacheType { get; set; }
        public string CacheKey { get; set; }
        public object[] Paramsters { get; set; }

        public RequestInfo Load()
        {
            return this;
        }

        public StringBuilder Show()
        {
            return TemplateFactory.GetTemplateInstance(TemplateType).RanderTemplate();
        }
    }
    public class RequestInfo<T> : RequestInfo where T : ITemplate
    {
        public override Type TemplateType
        {
            get { return typeof(T); }
        }
    }
}
