
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
public static object[] Parameters(HttpContext c)
        
        { return null; }

        public static RequestInfo Request(){
            var parms = Parameters(HttpContext.Current);
            return Request((int)parms[0],(double)parms[1],(Index)parms[2]);
        }      



        public static RequestInfo Request(int i, double d, Index idx)
        {
            
    var ii = i + d;
    string stype = idx.GetType().FullName;
    return new RequestInfo<Login>();

        }

        public static StringBuilder Rander()
        {
            StringBuilder html = new StringBuilder();
            Echo(html, @"<div>
    <span>姓名：张三</span> <span>年龄：");
            Echo(html, Settings.aa + 1203);
            Echo(html, @"</span>
</div>
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
