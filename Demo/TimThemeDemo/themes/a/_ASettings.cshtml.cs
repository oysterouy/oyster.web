
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
        public _ASettings()
        {
                        
        }

        public static readonly int _loadingTimeout = 200;
        public static readonly string _themeName = "AAAA";
        public static readonly string _themeRelactivePath = "/templates/defaulttheme";

        static _ASettings()
        {
            //******** route setting *********//
             RouteManager.Instance.Route<_ASettings,timthemedemo.themes.a.Index>("/", "/");
            RouteManager.Instance.Route<_ASettings,timthemedemo.themes.a.Index>("/index", "/index/{0}-_-{1}/", "name", "age");
            RouteManager.Instance.Route<_ASettings,timthemedemo.themes.a.Index>("/idx", "/idx/{0}", "n");
            RouteManager.Instance.Route<_ASettings>((request) =>
{
    return timthemedemo.themes.a.Index.Instance;
});


            //******** filter setting *********//
 
        }

        public override string ThemeName{get{ return _themeName;}}
        public override int LoadingTimeout{get{ return _loadingTimeout;}}
        public override string ThemeRelactivePath{get{ return _themeRelactivePath;}}
    }
}
