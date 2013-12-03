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
        public static string Write(string format, params object[] args)
        {
            if (args == null || args.Length == 0)
                return format;
            return string.Format(format, args);
        }
    }
}
