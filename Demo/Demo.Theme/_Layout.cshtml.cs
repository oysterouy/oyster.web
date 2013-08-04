
namespace demotheme
{
    using oyster.web;
    using oyster.web.define;
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
            invorker.Invork(typeof(_layout),"Body");
            Echo(html, @"
            </div>
            <div>
                <h2>
                    Foot 开始</h2>
                ");
            invorker.Invork(typeof(_layout),"Foot");
            Echo(html, @"
            </div>
            ");int cc = t + 1000;
            Echo(html, @"
            <a></a>
        ");
        
            Echo(html, @"<p>");
            Echo(html, response.BlockRander<Login>("LLL",true));
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

        
        public override object[] Init(Request request)
        {
            request.BlockRegister<Login>("LLL");

            
    request.Body.Paramters = new object[] { 1, 2, "str" };
    return request.Body.Paramters;

        }
        
        public dynamic RequestInternal(int index, int count, Response resp)
        {
            
    resp.BlockInvork<Login>("LLL",Login.Parameters("BBB"));
    return resp.Model;

        }

        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            var model= RequestInternal((int)parms[0],(int)parms[1],response);
            if (response.Model != model)
                throw new Exception("Please Set Model To Response.Model!");
        }
        
        public static object[] Parameters(int p0,int p1)
        {
            return new object[] {p0,p1 };
        }

    }
}
