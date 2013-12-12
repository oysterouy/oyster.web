using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.A.utility;

namespace oyster.web.A.contract
{
    [Serializable]
    public class Request
    {
        public string Path { get; set; }
        public KeyValueCollection<string, string> Paramters { get; set; }
    }
}
