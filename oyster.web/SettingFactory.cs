using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Linq.Expressions;

namespace oyster.web
{
    public class SettingFactory
    {
        public static string Field<T>(Expression<Func<T, T>> exp)
        {
            return "";
        }

        public static string Route(Func<HttpContext, ITemplate> Route)
        {
            return "";
        }

        public static string Filter(FilterOn onf,
            Func<HttpContext, ITemplate, StringBuilder, bool> filter)
        {
            return "";
        }

        public static string Init<T1>(Action<T1, string> func)
        {
            return "";
        }
        public static string Init<T1, T2>(Action<T1, T2> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3>(Action<T1, T2, T3> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4>(Action<T1, T2, T3, T4> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> func)
        {
            return "";
        }

        public static string Paramters(Func<HttpContext, ParamtersInfo> func)
        {
            return "";
        }
    }
}
