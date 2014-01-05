
namespace timthemedemo.themes.a
{
    using oyster.web;
    using oyster.web.manage;
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class _ASettings : TimTheme
    {
        public static readonly int _loadingTimeout = 0;
        public static readonly string _themeName = "AAAA";
        public static readonly string _themeRelactivePath = "/themes/a";

        public _ASettings()
        {
                         
        }

        public override void Init(){
            if(HadInit)
                return;
            //******** route setting *********//
            Route.Add<timthemedemo.themes.a.Index>("/", "/");
            Route.Add<timthemedemo.themes.a.Index>("/index", "/index/{0}-_-{1}/", "name", "age");
            Route.Add<timthemedemo.themes.a.Index>("/idx", "/idx/{0}", "n");
            Route.Add((request) =>
{
    return timthemedemo.themes.a.Index.Instance;
});

            //******** filter setting *********//

            HadInit = true;
        }
        public override string ThemeName{get{ return _themeName;}}
        public override int LoadingTimeout{get{ return _loadingTimeout;}}
        public override string ThemeRelactivePath{get{ return _themeRelactivePath;}}
    }
}
