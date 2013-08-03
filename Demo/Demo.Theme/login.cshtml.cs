
namespace demotheme
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Login : TemplateBase<Login>
    {
        static Login()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<div>
    <span>姓名：张三</span> <span>年龄：");
            Echo(html, Settings.aa + 1203);
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
            
    return new object[] { 1 };

        }
        
        public dynamic RequestInternal(string req, Response resp)
        {
            
    return resp.Model;

        }

        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            var model= RequestInternal((string)parms[0],response);
            if (response.Model != model)
                throw new Exception("Please Set Model To Response.Model!");
        }

    }
}
