
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Login : ITemplate
    {
public static object[] Parameters(Request c)
        
        { return new object[] { 1, 4.2, new Index() }; }

        dynamic ITemplate.Init(Request request)
        {
            return Init(request);
        }

        public static dynamic Init(Request request){
            var parms = Parameters(request);
            return Init((int)parms[0],(double)parms[1],(Index)parms[2]);
        }      

        public static dynamic Init(int i, double d, Index idx)
        {
            
    var ii = i + d;
    string stype = idx.GetType().FullName;
    return new { Type = stype };

        }
        void ITemplate.Request(Request request,Response response)
        {
            Request(request,response);
        }

        public static void Request(Request req,Response  resp)
        {
             
        }


        public static StringBuilder Rander(dynamic Model)
        {
            StringBuilder html = new StringBuilder();
            Echo(html, @"<div>
    <span>姓名：张三</span> <span>年龄：");
            Echo(html, Settings.aa + 1203);
            Echo(html, @"</span>
</div>
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
