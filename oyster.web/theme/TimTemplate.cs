using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public abstract class TimTemplate : TimTemplateBase
    {
        public abstract object[] Init(Request request);

        internal void Init(TimProcess timProcess, params object[] args)
        {
            if (args != null && args.Length > 0)
                timProcess.Response.Paramters = args;
            else
                timProcess.Response.Paramters = Init(timProcess.Request);
        }

        public abstract void Request(TimProcess timProcess);

        //public void Request(TimProcess timProcess)
        //{
        //    //根据参数数量选择执行的Request
        //    Request(0, "", timProcess.Response);

        //}

        //public void Request(int i, string s, Response response)
        //{

        //}

        internal void Render(TimProcess timProcess)
        {
            throw new NotImplementedException();
        }
    }
    public abstract class TimTemplate<T> : TimTemplate
    where T : TimTemplate
    {
        protected static readonly Dictionary<string, Action<StringBuilder, Response, TimSection>> templateSections = new Dictionary<string, Action<StringBuilder, Response, TimSection>>();

        public static T Instance { get { return InstanceHelper<T>.Instance; } }
    }
}
