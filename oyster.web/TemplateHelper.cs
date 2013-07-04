using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using oyster.web;
using System.Runtime.Remoting.Messaging;

namespace oyster.web
{
    public class TemplateHelper
    {
        public static string Config<T>(Func<T, T> exp)
        {
            return "";
        }

        public static string Route(Func<HttpContext, ITemplate> Route)
        {
            return "";
        }

        public static string Filter(FilterOnEnum onf,
            Func<HttpContext, ITemplate, StringBuilder, bool> filter)
        {
            return "";
        }

        public static string Request<T1>(Func<T1, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2>(Func<T1, T2, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3>(Func<T1, T2, T3, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4>(Func<T1, T2, T3, T4, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, RequestInfo> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, RequestInfo> func)
        {
            return "";
        }

        public static string Parameters(Func<HttpContext, object[]> func)
        {
            return "";
        }

        public static void SetDataToContext<T>(string key, T data)
        {
            CallContext.SetData(key, data);
        }

        public static T GetDataFromContext<T>(string key)
        {
            return (T)CallContext.GetData(key);
        }
    }
}
