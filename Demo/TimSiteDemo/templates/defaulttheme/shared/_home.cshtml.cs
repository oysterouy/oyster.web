
namespace timsitedemo.templates.defaulttheme.shared
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class _home : TemplateBase<_home>
    {
        static _home()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
<!DOCTYPE html>
<html>
<head>
    <meta name=""viewport"" content=""width=device-width"" />
    <link rel=""stylesheet"" href=""");
            Echo(html, Src("/context/bootstrapv3.03/css/bootstrap.min.css"));
            Echo(html, @""" />
    <title></title>
</head>
<body>
    ");
            invorker.Invoke(typeof(_home),"Body");
            Echo(html, @"
    <script src=""");
            Echo(html, Src("/context/jquery/jquery-1.10.2.min.js"));
            Echo(html, @"""></script>
    <script src=""");
            Echo(html, Src("/context/bootstrapv3.03/js/bootstrap.min.js"));
            Echo(html, @"""></script>
    ");
            invorker.Invoke(typeof(_home),"Script");
            Echo(html, @"
</body>
</html>
");
            invorker.Invoke(typeof(_home),"Foot");});
templateSections.Add("Body",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
        <h1>
            _home layout body!</h1>
   ");});
templateSections.Add("Script",(html,response,invorker)=>{
    dynamic Model=response.Model;
});
templateSections.Add("Foot",(html,response,invorker)=>{
    dynamic Model=response.Model;
});

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
