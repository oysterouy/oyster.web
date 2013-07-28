
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class _layout : ITemplate
    {
        static Dictionary<string, List<string>> htmlBlockPool = new Dictionary<string, List<string>>();
        static Dictionary<string, Action<StringBuilder>> sectionBlockPool = new Dictionary<string, Action<StringBuilder>>();
        static Dictionary<KeyValuePair<string, int>, KeyValuePair<string, Type>> childTemplates = new Dictionary<KeyValuePair<string, int>, KeyValuePair<string, Type>>();

        static _layout()
        {
            sectionBlockPool.Add("Body",(html)=>{
            Echo(html, @"<div>
                        默认Body!!!</div>
               ");});
sectionBlockPool.Add("Foot",(html)=>{
            Echo(html, @"<p>
                        我是默认脚</p>
               ");});

        }
public static object[] Parameters(Request req)
        
        {
    return new object[] { };
}

        dynamic ITemplate.Init(Request request)
        {
            return Init(request);
        }

        public static dynamic Init(Request request){
            var parms = Parameters(request);
            return Init((long)parms[0],(string)parms[1]);
        }      

        public static dynamic Init(long userId, string name)
        {
            
    return null;

        }
        void ITemplate.Request(Request request,Response response)
        {
            Request(request,response);
        }

        public static void Request(Request req,Response  resp)
        {
            
    //req.Block<Login>(("AAAAAAAAAAAAAAAAAAAA")=>{
    //return null;
    //});

        }


        public static StringBuilder Rander(dynamic Model)
        {
            StringBuilder html = new StringBuilder();
            Echo(html, @"<!DOCTYPE html PUBLIC ""-//W3C//DTD XHTML 1.0 Transitional//EN"" ""http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd"">
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <title></title>
</head>
<body>
    <h1>
        我是布局页面的标题</h1>
    <div>
        ");
            var t = 0;
            t += 10;
            
            Echo(html, @"<div>
                <h2>
                    Body开始了</h2>
                ");
            Echo(html, @"
            </div>
            <div>
                <h2>
                    Foot 开始</h2>
                ");
            Echo(html, @"
            </div>
        }
        <p>");
            Echo(html, @"
        </p>
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
