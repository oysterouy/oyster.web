
namespace timsitedemo.templates.defaulttheme
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;
    using timsitedemo.templates.defaulttheme;
    using timsitedemo.templates.defaulttheme.shared;

    public class Index : TemplateBase<Index>
    {
        static Index()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            invorker.Invoke(typeof(Index),"Body");
            invorker.Invoke(typeof(Index),"Script");});
templateSections.Add("Body",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
    <div>
        <p>
            I amd Index!</p>
    </div>
");});
templateSections.Add("Script",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
    <script language=""javascript"" type=""text/javascript"">
        alert(jQuery);
    </script>
");});

        }

        
        public override object[] Init(Request request)
        {
            
            
    request.Layout<_home>();
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
