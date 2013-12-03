
namespace timsitedemo
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using timsitedemo;

    public class Index : TemplateBase<Index>
    {
        static Index()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
<!DOCTYPE html>
<html>
<head>
    <meta name=""viewport"" content=""width=device-width"" />
    <title>");
            Echo(html, Write("Title"));
            Echo(html, @"</title>
</head>
<body>
    <div>");
            Echo(html, Write("{0}-{1}-dsdda", 1, 3));
            Echo(html, @"
    </div>
    <p>
        <a href=""");
            Echo(html, Url<Index>("oyster"));
            Echo(html, @""">Title</a>
    </p>
    <p>
        <a href=""");
            Echo(html, Url<Index>("oyster", 4));
            Echo(html, @""">Title1</a>
    </p>
</body>
</html>
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
