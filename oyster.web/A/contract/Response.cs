using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.A.contract
{
    [Serializable]
    public class Response
    {
        public object[] Paramters { get; set; }
        public dynamic Model { get; set; }
    }
}
