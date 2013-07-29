
namespace demotheme
{
    using oyster.web;
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
</div>
");});

        }

        
        public override dynamic Init(Request request)
        {
            
    return new { Type = 1 };

        }
        
        public override void Request(Request req,Response  resp)
        {
             
        }

    }
}
