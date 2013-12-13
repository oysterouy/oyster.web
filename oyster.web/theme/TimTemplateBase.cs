using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;

namespace oyster.web
{
    public abstract class TimTemplateBase
    {
        /// <summary>
        /// Response.Model
        /// </summary>
        public dynamic Model { get; private set; }
        public TimTemplate LayoutTemplate { get; protected set; }

        public static string Write(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                return format;
            return string.Format(format, args);
        }

        public static string Url<T>(string pathStart, object[] args)
            where T : TimTemplate
        {
            var process = TimProcessContext.GetProcess();
            return process.Theme.Route.Url<T>(pathStart, args);
        }
        public static string Url<T>(params object[] args)
            where T : TimTemplate
        {
            return Url<T>(null, args);
        }
        public static string Src(string fileName)
        {
            var process = TimProcessContext.GetProcess();
            return process.Theme.Route.Src(fileName);
        }

        public void Layout<T>(params object[] args) where T : TimTemplate
        {
            LayoutTemplate = InstanceHelper<T>.Instance;
        }
    }
}
