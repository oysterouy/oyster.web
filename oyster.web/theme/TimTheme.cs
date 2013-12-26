using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.manage;
using System.Collections.Concurrent;

namespace oyster.web
{
    public abstract partial class TimTheme
    {
        public TimTheme()
        {
            Route = RouteFactory.Create(this, null);
        }
        public abstract string ThemeName { get; }
        public abstract int LoadingTimeout { get; }
        public abstract string ThemeRelactivePath { get; }

        public TimRoute Route { get; protected set; }
        TimTheme _baseTheme;
        public TimTheme BaseTheme
        {
            get { return _baseTheme; }
            protected set
            {
                if (value != null)
                    Route.SetBase(value.Route);
                _baseTheme = value;
            }
        }
        public void SetBase(TimTheme baseTheme)
        {
            BaseTheme = baseTheme;
        }
    }
}
