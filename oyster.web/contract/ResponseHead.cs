using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    [Serializable]
    public class ResponseHead
    {
        public ResponseHead()
        {
            StatusCode = 200;
        }
        public int StatusCode { get; set; }
    }
}
