
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Settings : ISetting
    {
        public static readonly int i = 6;
        public static readonly string Name = "sdljflasdjfljwel,sd";
        public static readonly ISetting ii = new demotheme.Settings();
        public static readonly decimal aa = 6.978879M;

        static readonly List<Func<HttpContext, ITemplate>> routes = new List<Func<HttpContext, ITemplate>>();

        static readonly Dictionary<FilterOnEnum,
            List<Func<HttpContext, ITemplate, StringBuilder, bool>>> filterDic =
            new Dictionary<FilterOnEnum, List<Func<HttpContext, ITemplate, StringBuilder, bool>>>();

        static Settings()
        {
            //******** route setting *********//
            routes.Add((context) =>
{
    return new Index();
});


            //******** filter setting *********//

            var ls0 = new List<Func<HttpContext, ITemplate, StringBuilder, bool>>();
            filterDic.Add(FilterOnEnum.BeforeRequest, ls0);
            ls0.Add( (context, it, str) =>
{
    return true;
});

        }

        ITemplate ISetting.Route(HttpContext context)
        {
            foreach (var rt in routes)
            {
                var it = rt(context);
                if (it != null)
                    return it;
            }
            return null;
        }

        bool ISetting.Filter(FilterOnEnum fon, HttpContext context, ITemplate it, StringBuilder str)
        {
            List<Func<HttpContext, ITemplate, StringBuilder, bool>> ls = null;
            if (filterDic.TryGetValue(fon, out ls))
            {
                foreach (var f in ls)
                {
                    if (!f(context, it, str))
                        return false;
                }
            }
            return true;
        }
    }
}
