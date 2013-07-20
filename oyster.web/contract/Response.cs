using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    [Serializable]
    public class Response
    {
        public ResponseHead Head { get; set; }

        [NonSerialized]
        public dynamic Model = null;

        public StringBuilder Body { get; set; }

        public void Waiting(int millisecond)
        {

        }
    }
}
