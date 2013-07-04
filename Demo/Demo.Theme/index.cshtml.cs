
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Index : ITemplate
    {
public static object[] Parameters(HttpContext context)
        
        {
    return new object[4];
}

        public static RequestInfo Request(){
            var parms = Parameters(HttpContext.Current);
            return Request((string)parms[0],(int)parms[1]);
        }      



        public static RequestInfo Request(string s, int t)
        {
            
    var sss = s.Length + t;

    var d = Login.Request(0, 312, null).Load();
    TemplateHelper.SetDataToContext("Login", d);
    return new RequestInfo<Index>
    {
    };

        }

        public static StringBuilder Rander()
        {
            StringBuilder html = new StringBuilder();
    var tttt = 12;

            Echo(html, @"
<!--page:-->
<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <title>index</title>
</head>
<body>
    ");
        var ttt = Settings.i + 1 + tttt;
    
            Echo(html, @"
    <h1>
        Hello world~</h1>
    <div>
        <!--page:20130812012234-logininfo-fawqdsdsdmwemwekqlwqw21dsd
            <div></div>
        -->
        ");
            var login = TemplateHelper.GetDataFromContext<RequestInfo>("Login");            
        
            Echo(html, login.Show());
            Echo(html, @"
    </div>
    <div>
    </div>
</body>
</html>
");
            //container.RanderResult=html;
            return html;
        }

        internal  static  StringBuilder Echo(StringBuilder html, object p)
        {
            html.Append(p == null ? "" : p.ToString());
            return html;
        }

        StringBuilder ITemplate.RanderTemplate()
        {
           return Rander();
        }

        RequestInfo ITemplate.RequestTemplate()
        {
            return Request();
        }
    }
}
