using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.A.contract;
using oyster.web.A.theme;

namespace oyster.web.A.implement
{
    class TimProcess
    {
        public TimProcess(TimOrder order, TimHost host)
        {
            Order = order;
            Host = host;
        }
        TimOrder Order { get; set; }
        TimHost Host { get; set; }
        public void Process()
        {
            TimProcessContext.SetProcess(this);

            var temp = RouteManager.Instance.Match(Order.Request);
            if (temp == null)
            {
                response = DefaultResponseManager.GetResponse(404);
                goto outmethod;
            }

            var theme = Host.GetTheme(Order.Request);


        }
    }
}
