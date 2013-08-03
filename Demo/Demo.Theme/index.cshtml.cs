
namespace demotheme
{
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

    var tttt = 12;

    var ttt = Settings.i + 1 + tttt + Model.Index;

            Echo(html, @"
<h1>
    Hello world~ 我是继承页的BODY</h1>
<div>
</div>
<div>
    <p>
        哈哈哈哈</p>
    <div>
        <b>加载Login..</b>
        ");
            Echo(html, response.Block<Login>("Index_Login",false));
            Echo(html, @"
    </div>
</div>
");
            invorker.Invork(typeof(Index),"Foot");});
templateSections.Add("Foot",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<div>
        <h2>
            我是继承页页脚</h2>
    </div>
");});

        }

        
        public override object[] Init(Request request)
        {
            request.InvorkBlock<Login>("Index_Login");

            
    request.Layout<_layout>();
    request.BlockModel<Login>("Index_Login",Login.Parameters("AAA"));
    return new object[] { 1, 3 };

        }
        
        public dynamic RequestInternal(int idx, Response response)
        {
            
    int t = 5;
    response.SetLayoutModel<_layout>((md) =>
    {
        md.Title = "AAAAAAAAAAA";
        return md;
    });
    response.Model.Index = 3;
    return response.Model;

        }

        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            var model= RequestInternal((int)parms[0],response);
            if (response.Model != model)
                throw new Exception("Please Set Model To Response.Model!");
        }
        
        public static object[] Parameters(int p0)
        {
            return new object[] {p0 };
        }

    }
}
