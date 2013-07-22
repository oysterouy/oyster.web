using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    [Serializable]
    public class Response
    {
        public Response(ITemplate temp)
        {
            Template = temp;
        }
        public ITemplate Template { get; protected set; }

        public ResponseHead Head { get; set; }

        [NonSerialized]
        public dynamic Model = null;

        public StringBuilder Body { get; set; }

        [NonSerialized]
        public Exception Exception = null;

        internal System.Threading.AutoResetEvent waitHandle { get; set; }
        public Response Waiting(int millisecond = -1)
        {
            if (millisecond < 0)
                millisecond = TemplateManager.GetSettingFromTemplate(Template).LoadingTimeout;

            if (waitHandle != null)
            {
                waitHandle.WaitOne(millisecond);
            }
            if (Exception != null)
                throw Exception;

            return this;
        }

        public Response Rander()
        {
            Waiting();
            Body = Template.Rander(Model);
            return this;
        }
    }

    public class Response<T> : Response
         where T : ITemplate, new()
    {
        public Response()
            : base(new T())
        {
        }
    }
}
