
namespace timsitedemo.templates.defaulttheme
{
    using oyster.web;
    using oyster.web.manage;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using timsitedemo.templates.defaulttheme;

    public class DefaultSettings : TimTheme
    {
        public DefaultSettings()
        {
                        
        }


        static DefaultSettings()
        {
            //******** route setting *********//
 

            //******** filter setting *********//
 
        }

        public override string ThemeName{get{ return _themeName;}}
        public override int LoadingTimeout{get{ return _loadingTimeout;}}
        public override string ThemeRelactivePath{get{ return _themeRelactivePath;}}
    }
}
