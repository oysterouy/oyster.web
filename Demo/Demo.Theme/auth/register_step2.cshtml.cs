
namespace demotheme.auth
{
    using oyster.web;
    using demotheme;
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Register_step2 : TemplateBase<Register_step2>
    {
        static Register_step2()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
<div>
    <span>姓名：");
            Echo(html, Model.Name);
            Echo(html, @"</span> <span>年龄：");
            Echo(html, Hostsettings.aa + 1203);
            Echo(html, @"</span>
    ");
        for (int i = 0; i < 6; i++)
        {
        
            Echo(html, @"<p>");
            Echo(html, i);
            Echo(html, @"</p>
        <div>
            aaaa</div>
        ");i += 2;}
         
            Echo(html, @"
        <p>
            HHHHHHHHHHHHh</p> ");

            Echo(html, @"</div>
");});

        }

        
        public override object[] Init(Request request)
        {
            
            
    if(request.Head.Method=="GET")
        return new object[0];
    else 
    return new object[] { "张三"};

        }
        
        public void RequestInternal(Response response)
        {
            


        }

        public static object[] Parameters()
        {
            return new object[] { };
        }
        public void RequestInternal(string str , Response response)
        {
            


        }

        public static object[] Parameters(string p0)
        {
            return new object[] {p0 };
        }
        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            if(parms==null)
                throw new Exception("Paramters no set!");
            
            if(parms.Length==0)
                RequestInternal(response);
            if(parms.Length==1)
                RequestInternal((string)parms[0],response);        
        }
    }
}
