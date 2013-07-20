using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public RequestHead Head { get; set; }
        public object Body { get; set; }

        public Response Execute()
        {
            return null;
        }
    }
}
