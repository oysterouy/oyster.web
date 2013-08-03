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

        public static string FilterBeforeRequest(Func<Request, bool> filter)
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

        public static string Request<T1>(Func<T1, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2>(Func<T1, T2, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3>(Func<T1, T2, T3, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4>(Func<T1, T2, T3, T4, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5>(Func<T1, T2, T3, T4, T5, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6>(Func<T1, T2, T3, T4, T5, T6, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7>(Func<T1, T2, T3, T4, T5, T6, T7, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8>(Func<T1, T2, T3, T4, T5, T6, T7, T8, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8, T9>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, Response, dynamic> func)
        {
            return "";
        }
        public static string Request<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, Response, dynamic> func)
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
