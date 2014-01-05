
namespace timthemedemo.themes.a.shared
{
    using oyster.web;
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class _Home : TimTemplate<_Home>
    {
        static _Home()
        {
            TemplateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
<!DOCTYPE html>
<html>
<head>
    <meta name=""viewport"" content=""width=device-width"" />
    <title>");
            Echo(html, Model.Head.Title);
            Echo(html, @"</title>
    <script language=""javascript"" type=""text/javascript"" src=""");
            Echo(html, Src("/resource/jquery/jquery-1.10.2.min.js"));
            Echo(html, @"""></script>
    ");
            invorker.Invoke(typeof(_Home),"Head");
            Echo(html, @"
</head>
<body>
    <div>
        ");
            invorker.Invoke(typeof(_Home),"Body");
            Echo(html, @"
    </div>
    ");
            invorker.Invoke(typeof(_Home),"Script");
            Echo(html, @"
</body>
</html>
");});
TemplateSections.Add("Head",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
        <!-- empty layout head!-->
   ");});
TemplateSections.Add("Body",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
            <!-- empty layout body!-->
       ");});
TemplateSections.Add("Script",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
        <!-- empty layout script!-->
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
        public override void Request(TimProcess timProcess)
        {
            var response = timProcess.Response;
            object[] parms = response.Paramters;
            if(parms==null)
                throw new Exception("Paramters no set!");
            
            if(parms.Length==0)
                RequestInternal(response);        
        }
    }
}
