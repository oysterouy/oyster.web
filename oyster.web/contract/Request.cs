using System;
using System.Collections.Generic;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public string Path { get; set; }
        public List<KeyValuePair<string, string>> Paramters { get; set; }
    }
}
