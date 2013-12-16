using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.WebPages;

namespace oyster.web
{
    public abstract class TimTemplateBase
    {
        static TimProcess Process { get { return TimProcessContext.GetProcess(); } }
        /// <summary>
        /// Response.Model
        /// </summary>
        public dynamic Model { get { return Process.Response.Model; } }
        public TimTemplate LayoutTemplate { get { return Process.Layout.Template; } }

        public static void Echo(StringBuilder sbuilder, string format, params object[] args)
        {
            if (args != null && args.Length > 0)
                sbuilder.AppendFormat(format, args);
            else
                sbuilder.Append(format);
        }
        public static string Write(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                return format;
            return string.Format(format, args);
        }

        public static string Url<T>(string pathStart, object[] args)
            where T : TimTemplate
        {
            return Process.Theme.Route.Url<T>(pathStart, args);
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

        public bool Layout<T>(params object[] args) where T : TimTemplate
        {
            return Process.SetLayout(InstanceHelper<T>.Instance, args);
        }

        public string Block<T>(string callID) where T : TimTemplate
        {
            return Process.BlockRender(InstanceHelper<T>.Instance, callID);
        }
        public void BlockInvoke<T>(string callID, params object[] args) where T : TimTemplate
        {
            Process.BlockInvoke(InstanceHelper<T>.Instance, callID, args);
        }
    }
}
