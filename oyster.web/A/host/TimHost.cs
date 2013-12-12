using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.A.contract;
using oyster.web.A.theme;
using oyster.web.A.implement;

namespace oyster.web.A.contract
{
    public class TimHost
    {
        private TimHost()
        {

        }
        static TimHost _instance;
        public static TimHost Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new TimHost();
                }
                return _instance;
            }
        }

        public virtual Response Execute(Request request)
        {
            var order = new TimOrder();
            var process = new TimProcess(order, this);
            process.Process();
            return order.Response;
        }

        public abstract TimTheme GetTheme(Request request);
    }
}
