using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.codegenerator
{
    class Config
    {
        static readonly string razorProjectGuid = "{E3E379DF-F4C6-4180-9B81-6769533ABE47}";
        static readonly string razorConfigType = "System.Web.WebPages.Razor.Configuration.RazorWebSectionGroup, System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35";
        static readonly string razorPagesConfigType = "System.Web.WebPages.Razor.Configuration.RazorPagesSection, System.Web.WebPages.Razor, Version=2.0.0.0, Culture=neutral, PublicKeyToken=31BF3856AD364E35";

        static string _razorProjectGuid = null;
        public static string RazorProjectGuid
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_razorProjectGuid))
                    return razorProjectGuid;

                return _razorProjectGuid;
            }
            set
            {
                _razorProjectGuid = value.StartsWith("{") ? value : "{" + value + "}";
            }
        }

        static string _razorConfigType = null;
        public static string RazorConfigType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_razorConfigType))
                    return razorConfigType;

                return _razorConfigType;
            }
            set
            {
                _razorConfigType = value;
            }
        }

        static string _razorPagesConfigType = null;
        public static string RazorPagesConfigType
        {
            get
            {
                if (string.IsNullOrWhiteSpace(_razorPagesConfigType))
                    return razorPagesConfigType;

                return _razorPagesConfigType;
            }
            set
            {
                _razorPagesConfigType = value;
            }
        }

    }
}
