
namespace demosite.Themes.WhiteBlue
{
    using oyster.web;
    using demosite.Themes.WhiteBlue.Layout;
    using oyster.web;
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

            Echo(html, @"
<div>
    <h1>
        Hello world!</h1>
    <div>
        ");
            
            Echo(html, @"<a href>dsd</a>            
            <p>
                ");
                    if (Model.I != null && Model.I)
                    {
                    
            Echo(html, @"<a href="""">asas</a>
                    ");}
                
            Echo(html, @"
            </p>
        ");
    
            Echo(html, @"</div>
</div>
");
            invorker.Invoke(typeof(Index),"OutHtml");});
templateSections.Add("OutHtml",(html,response,invorker)=>{
    dynamic Model=response.Model;

        if (Model.IsSuccess != null && Model.IsSuccess)
        {
        
            Echo(html, @"<script type=""text/javascript"">
            alert(1);
        </script>
        ");}
        else
        {
        
            Echo(html, @"<script type=""text/javascript"">
            alert(2);
        </script>
        ");}
    });

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
