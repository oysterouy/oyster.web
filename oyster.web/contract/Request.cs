using System;
using System.Collections.Generic;

namespace oyster.web
{
    [Serializable]
    public class Request
    {
        public Uri RequestUrl { get; set; }
        public Uri RequestUrlReferrer { get; set; }
        public string Path { get { return RequestUrl.AbsolutePath; } }
        public List<KeyValuePair<string, string>> Paramters { get; set; }
        public int LoadingTimeout { get; set; }
    }
}
