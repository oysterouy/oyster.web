using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using oyster.web.host;
using timthemedemo.themes.a;

namespace TimThemeDemo.handle
{
    public class MainHost : TimHost
    {
        static _ASettings ASetting = new _ASettings();
        public override oyster.web.TimTheme GetTheme(oyster.web.Request request)
        {
            return ASetting;
        }
        public override oyster.web.TimTheme GetTheme(string themeName)
        {
            switch (themeName.ToLower())
            {
                case "aaaa":
                    return ASetting;

                default: return ASetting;
            }
        }
    }
}