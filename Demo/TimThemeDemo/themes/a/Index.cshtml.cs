
namespace timthemedemo.themes.a
{
    using oyster.web;
    using oyster.web;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Index : TimTemplate<Index>
    {
        static Index()
        {
            TemplateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"
<div>
    <h1>
        Demo Index</h1>
    <div>
        <img src=""");
            Echo(html, Src("/resource/img/demo.gif"));
            Echo(html, @""" alt="""" />
    </div>
</div>
");
            invorker.Invoke(typeof(Index),"Script");});
TemplateSections.Add("Script",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<script language=""javascript"" type=""text/javascript"" src=""");
            Echo(html, Src("/resource/js/home.js"));
            Echo(html, @"""></script>
");});

        }

        
        public override object[] Init(Request request)
        {
            
            
    Layout<timthemedemo.themes.a.shared._Home>();

    return new object[0];

        }
        
        public void RequestInternal(Response response)
        {
            


        }

        public static object[] Parameters()
        {
            return new object[] { };
        }
        public void RequestInternal(string u ,  Response response)
        {
            


        }

        public static object[] Parameters(string p0)
        {
            return new object[] {p0 };
        }
        public override void Request(TimProcess timProcess)
        {
            var response = timProcess.Response;
            object[] parms = response.Paramters;
            if(parms==null)
                throw new Exception("Paramters no set!");
            
            if(parms.Length==0)
                RequestInternal(response);
            if(parms.Length==1)
                RequestInternal((string)parms[0],response);        
        }
    }
}
