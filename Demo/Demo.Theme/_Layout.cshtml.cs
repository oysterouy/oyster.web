
namespace demotheme
{
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class _layout : TemplateBase<_layout>
    {
        static _layout()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

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
            invorker.Invork("Body");
            Echo(html, @"
            </div>
            <div>
                <h2>
                    Foot 开始</h2>
                ");
            invorker.Invork("Foot");
            Echo(html, @"
            </div>
        }
        <p>");
            Echo(html, @"
        </p>
    </div>
</body>
</html>
");});
templateSections.Add("Body",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<div>
                        默认Body!!!</div>
               ");});
templateSections.Add("Foot",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<p>
                        我是默认脚</p>
               ");});

        }

        
        public override dynamic Init(Request request)
        {
            
    return request.Body;

        }
        
        public override void Request(Request req,Response  resp)
        {
            


        }

    }
}
