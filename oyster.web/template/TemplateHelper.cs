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

        public static string Parameters(Func<HttpContext, object[]> func)
        {
            return "";
        }

        public static string Request(Action<Request, Response> func)
        {
            return "";
        }

        public static string FilterBeforeRoute(Func<HttpContext, bool> filter)
        {
            return "";
        }

        public static string FilterBeforeRequest(Func<HttpContext, ITemplate, Request, bool> filter)
        {
            return "";
        }

        public static string FilterBeforeRander(Func<HttpContext, ITemplate, Request, Response, bool> filter)
        {
            return "";
        }

        public static string FilterAfterRander(Func<HttpContext, ITemplate, Request, Response, bool> filter)
        {
            return "";
        }

        public static string Init<T1>(Func<T1, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2>(Func<T1, T2, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3>(Func<T1, T2, T3, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Request> func)
        {
            return "";
        }
        public static string Init<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Request> func)
        {
            return "";
        }
    }
}
