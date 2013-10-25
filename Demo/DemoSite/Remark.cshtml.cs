
namespace demosite
{
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Remark : TemplateBase<Remark>
    {
        static Remark()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<h1>
    神奇的Visual Studio,站点根目录下必须有个.cshtml的文件，否则子目录的.cshtml会映射不了编译绑定。 虽然我们不用他的绑定编译，但编辑页面时看着警告，确实不爽啊。
</h1>
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
