
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
    return new object[1];
}

        Request ITemplate.Init(HttpContext context)
        {
            return Init(context);
        }

        public static Request Init(HttpContext context){
            var parms = Parameters(context);
            return Init((string)parms[0]);
        }      

        public static Request Init(string pageIdx)
        {
            
    var resp = Login.Init(1, 23, new Index()).Execute();
    return new oyster.web.Request<Index>
    {
        Head = new oyster.web.RequestHead
        {
            CacheKey = "",
        },
        Body = new { A = 1, B = 3, login = resp }
    };

        }
        void ITemplate.Request(Request request,Response response)
        {
            Request(request,response);
        }

        public static void Request(Request request,Response  response)
        {
            
    int t = 5;
    response.Model = new { Index = request.Body.A + request.Body.B * 13, login = request.Body.login.Rander() };

        }


        public static StringBuilder Rander(dynamic Model)
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
        var ttt = Settings.i + 1 + tttt + Model.Index;
    
            Echo(html, @"
    <h1>
        Hello world~</h1>
    <div>
        ");
            Echo(html, Model.login.Body
);
            Echo(html, @"
    </div>
    <div>
    </div>
</body>
</html>
");
            return html;
        }
        
        StringBuilder ITemplate.Rander(dynamic Model)
        {
            return Rander(Model);
        }

        internal  static  StringBuilder Echo(StringBuilder html, object p)
        {
            html.Append(p == null ? "" : p.ToString());
            return html;
        }

        
    }
}
