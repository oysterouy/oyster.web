
namespace demosite.Themes.WhiteBlue
{
    using oyster.web;
    using demosite.Themes.WhiteBlue.Layout;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Index : TemplateBase<Index>
    {
        static Index()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<div>
    <h1>
        Hello world!</h1>
</div>
");});

        }

        
        public override object[] Init(Request request)
        {
            
            
    request.Layout<_Home>();
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
