using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.manage
{
    public class RouteFactory
    {
        static Type routeType = typeof(TimRoute);
        public static void SetRouteType<T>()
        where T : TimRoute
        {
            routeType = typeof(T);
        }
        public static TimRoute Create(TimTheme theme, TimRoute baseRoute)
        {
            var r = Activator.CreateInstance(routeType, theme, baseRoute) as TimRoute;
            if (r == null)
                throw new Exception(string.Format("Can not Create TimRoute with route type of {0}!", routeType.FullName));
            return r;
        }
    }
}
