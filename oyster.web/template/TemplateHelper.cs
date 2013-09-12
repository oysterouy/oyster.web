using System;
using oyster.web.define;


namespace oyster.web
{
    public class TemplateHelper
    {
        public static string Config<T>(Func<T, T> exp)
        {
            return "";
        }

        public static string Route(Func<Request, TemplateBase> Route)
        {
            return "";
        }

        public static string FilterBeforeRoute(Func<Request, bool> filter)
        {
            return "";
        }

        public static string FilterBeforeRequest(Func<Request, Response, bool> filter)
        {
            return "";
        }

        public static string FilterBeforeRander(Func<Request, Response, bool> filter)
        {
            return "";
        }

        public static string FilterAfterRander(Func<Request, Response, bool> filter)
        {
            return "";
        }
        public static string Init(Func<Request, object[]> func)
        {
            return "";
        }

        public static string Request(Action<Response> func)
        {
            return "";
        }
        public static string Request<T1>(Action<T1, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2>(Action<T1, T2, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3>(Action<T1, T2, T3, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4>(Action<T1, T2, T3, T4, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5>(Action<T1, T2, T3, T4, T5, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6>(Action<T1, T2, T3, T4, T5, T6, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7>(Action<T1, T2, T3, T4, T5, T6, T7, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8>(Action<T1, T2, T3, T4, T5, T6, T7, T8, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, Response> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Response> func)
        {
            return "";
        }

        public static string Block<T>(Func<T, string, bool> act)
            where T : TemplateBase
        {
            return "";
        }
    }
}
