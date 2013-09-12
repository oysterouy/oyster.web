
namespace demotheme.auth
{
    using oyster.web;
    using demotheme;
    using demotheme.auth;
    using oyster.web;
    using oyster.web.define;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web;

    public class Register : TemplateBase<Register>
    {
        static Register()
        {
            templateSections.Add("Page",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, response.BlockRander<Register_step1>("Step1",false));
            Echo(html, @"
            break;
            case 2:
    ");
            Echo(html, response.BlockRander<Register_step2>("Step2",false));
            Echo(html, @"
            break;
    ");});

        }

        
        public override object[] Init(Request request)
        {
            request.BlockRegister<Register_step1>("Step1");
request.BlockRegister<Register_step2>("Step2");

            
    if (request.Head.Method == "GET")
        return new object[0];
    else
        return new object[] { "张三" };

        }
        
        public void RequestInternal(Response response)
        {
            


        }

        public static object[] Parameters()
        {
            return new object[] { };
        }
        public void RequestInternal(string str ,  Response response)
        {
            


        }

        public static object[] Parameters(string p0)
        {
            return new object[] {p0 };
        }
        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            if(parms==null)
                throw new Exception("Paramters no set!");
            
            if(parms.Length==0)
                RequestInternal(response);
            if(parms.Length==1)
                RequestInternal((string)parms[0],response);        
        }
    }
}
