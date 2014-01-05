using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.hosting
{
    [Serializable]
    public class ResponseInfo
    {
        public ResponseHead Header { get; set; }
        public StringBuilder Body { get; set; }
    }
}
