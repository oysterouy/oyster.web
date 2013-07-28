
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
        static Dictionary<string, List<string>> htmlBlockPool = new Dictionary<string, List<string>>();
        static Dictionary<string, Action<StringBuilder>> sectionBlockPool = new Dictionary<string, Action<StringBuilder>>();
        static Dictionary<KeyValuePair<string, int>, KeyValuePair<string, Type>> childTemplates = new Dictionary<KeyValuePair<string, int>, KeyValuePair<string, Type>>();

        static Index()
        {
            sectionBlockPool.Add("Body",(html)=>{});

        }
public static object[] Parameters(Request context)
        
        {
    return new object[1];
}

        dynamic ITemplate.Init(Request request)
        {
            return Init(request);
        }

        public static dynamic Init(Request request){
            var parms = Parameters(request);
            return Init((Request)parms[0],(string)parms[1]);
        }      

        public static dynamic Init(Request request, string pageIdx)
        {
            
    request.Layout<_layout>(() => _layout.Init(1, ""));
    return new { A = 1, B = 3 };

        }
        void ITemplate.Request(Request request,Response response)
        {
            Request(request,response);
        }

        public static void Request(Request request,Response  response)
        {
            
    int t = 5;
    response.SetLayoutModel<_layout>((md) =>
    {
        md.Title = "AAAAAAAAAAA";
        return md;
    });
    response.Model = new { Index = request.Body.A + request.Body.B * 13 };

        }


        public static StringBuilder Rander(dynamic Model)
        {
            StringBuilder html = new StringBuilder();
    var tttt = 12;

            Echo(html, @"
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
