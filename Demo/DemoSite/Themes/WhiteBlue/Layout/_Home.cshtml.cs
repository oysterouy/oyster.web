
namespace demosite.Themes.WhiteBlue.Layout
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class _Home : TemplateBase<_Home>
    {
        static _Home()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
<!DOCTYPE html>
<html>
<head>
    <meta http-equiv=""Content-Type"" content=""text/html; charset=utf-8"" />
    <meta name=""description"" content=""");
            Echo(html, Model.Head.description);
            Echo(html, @""" />
    <meta name=""keywords"" content=""");
            Echo(html, Model.Head.keywords);
            Echo(html, @""" />
    <title>");
            Echo(html, Model.Head.title);
            Echo(html, @"</title>
    ");
            invorker.Invoke(typeof(_Home),"Head");
            Echo(html, @"
</head>
<body>
    ");
            invorker.Invoke(typeof(_Home),"Body");
            invorker.Invoke(typeof(_Home),"Foot");
            Echo(html, @"
</body>
</html>
");
            invorker.Invoke(typeof(_Home),"OutHtml");});
templateSections.Add("Head",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
        <!--html head-->
   ");});
templateSections.Add("Body",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
        <!--html body-->
   ");});
templateSections.Add("Foot",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
        <!--html foot-->
   ");});
templateSections.Add("OutHtml",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
    <!--out html-->
");});

        }

        
        public override object[] Init(Request request)
        {
            
            
    return new object[0];

        }
        
        public void RequestInternal(Response response)
        {
            


        }

        public static object[] Parameters()
        {
            return new object[] { };
        }
        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            if(parms==null)
                throw new Exception("Paramters no set!");
            
            if(parms.Length==0)
                RequestInternal(response);        
        }
    }
}
