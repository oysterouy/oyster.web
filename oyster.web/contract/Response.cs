using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public class Response
    {
        public Response()
        {
            Head = new ResponseHead();
        }
        internal ITemplate Template { get; set; }

        public ResponseHead Head { get; set; }

        public dynamic Model { get; set; }

        public StringBuilder Body { get; set; }

        public Exception Exception { get; set; }

        internal System.Threading.AutoResetEvent waitHandle { get; set; }

        public Response Waiting(int millisecond = -1)
        {
            if (millisecond < 0)
                millisecond = TemplateManager.GetSetting(Template.GetType().Assembly).LoadingTimeout;

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

        Response LayoutResponse { get; set; }

        public void SetLayoutModel<T>(Func<dynamic, dynamic> setLayoutModelAction)
        {
            LayoutResponse.Model = setLayoutModelAction(LayoutResponse.Model);
        }
    }
}
