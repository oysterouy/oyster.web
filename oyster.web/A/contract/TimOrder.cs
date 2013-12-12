using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.A.contract
{
    [Serializable]
    public class TimOrder
    {
        public Request Request { get; set; }
        public Response Response { get; set; }
        public TimTemplate Template { get; set; }
    }
}
