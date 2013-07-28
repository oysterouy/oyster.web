using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.hosting
{
    class DefaultResponseManager
    {
        public static Response GetResponse(int code)
        {
            return new Response { Head = new ResponseHead { StatusCode = code }, Body = new StringBuilder("Page No Found!") };
        }
    }
}
