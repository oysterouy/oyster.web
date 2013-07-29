using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;

namespace oyster.web
{
    public class Response
    {
        public Response()
        {
            Head = new ResponseHead();
            Model = new ExpandoObject();
        }
        internal TemplateBase Template { get; set; }

        public ResponseHead Head { get; set; }

        public dynamic Model { get; set; }

        public StringBuilder Body { get; set; }

        public Exception Exception { get; set; }

        internal System.Threading.AutoResetEvent waitHandle { get; set; }
        object waitLock = new object();
        bool hadWaitingBack = false;

        public Response Waiting(int misecond = -1)
        {
            int millisecond = -1;
            if (misecond < 0)
                millisecond = TemplateManager.GetSetting(Template.GetType().Assembly).LoadingTimeout;
            else
                millisecond = misecond;

            if (waitHandle != null && !hadWaitingBack)
            {
                if (LayoutResponse != null)
                {
                    LayoutResponse.Waiting(0);
                }
                lock (waitLock)
                {
                    if (millisecond == 0)
                        waitHandle.WaitOne();
                    else
                        waitHandle.WaitOne(millisecond);
                    hadWaitingBack = true;
                }
            }
            if (Exception != null)
                throw Exception;

            return this;
        }

        public Response Rander()
        {
            Body = new StringBuilder();
            return Rander(Body);
        }

        public Response LayoutResponse { get; set; }

        public void SetLayoutModel<T>(Func<dynamic, dynamic> setLayoutModelAction)
        {
            LayoutResponse.Model = setLayoutModelAction(LayoutResponse.Model);
        }

        public Response Rander(StringBuilder html)
        {
            return RanderInternal(html, null);
        }

        internal Response RanderInternal(StringBuilder html, Dictionary<string, InvorkInfo> action)
        {
            Waiting();
            var actDic = new Dictionary<string, InvorkInfo>();
            foreach (var actKv in Template.Sections)
            {
                actDic.Add(actKv.Key, new InvorkInfo { Response = this, Section = actKv.Value });
            }
            if (action != null)
            {
                foreach (var kv in action)
                {
                    var actKv = kv;
                    if (actKv.Key == "Page")
                    {
                        actKv = new KeyValuePair<string, InvorkInfo>("Body", kv.Value);
                    }
                    if (actDic.ContainsKey(actKv.Key))
                        actDic[actKv.Key] = actKv.Value;
                    else
                        actDic.Add(actKv.Key, actKv.Value);
                }
            }
            if (LayoutResponse != null)
            {
                LayoutResponse.RanderInternal(html, actDic);
            }
            else
            {
                var invorker = new SectionInvork { Html = html, Sections = actDic };
                invorker.Invork();
            }
            return this;
        }
    }
}
