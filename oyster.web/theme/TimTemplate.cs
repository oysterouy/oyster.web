using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public abstract class TimTemplate : TimTemplateBase
    {
        public abstract object[] Init(Request request);

        internal void Init(TimProcess timProcess)
        {
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

        internal void Rander(TimProcess timProcess)
        {
            throw new NotImplementedException();
        }
    }
}
