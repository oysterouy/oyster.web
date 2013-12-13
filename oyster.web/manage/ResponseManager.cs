using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.manage
{
    public class ResponseManager
    {
        static ResponseManager instance;
        public static ResponseManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new ResponseManager();
                return instance;
            }
        }
        public static void SetInstance(ResponseManager mgr)
        {
            instance = mgr;
        }
        public virtual Response GetResponseByStatusCode(int code)
        {
            return new Response
            {
                //Head = new ResponseHead { StatusCode = code }, Body = new StringBuilder("Page No Found!")
            };
        }
    }
}
