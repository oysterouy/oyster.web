
namespace demotheme
{
    using oyster.web;
    using demotheme;
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
            Echo(html, response.BlockRander<Login>("Index_Login",false));
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
            request.BlockRegister<Login>("Index_Login");

            
    request.Layout<_layout>();
    request.BlockInvork<Login>("Index_Login",Login.Parameters("AAA"));
    return new object[] { 1, 3 };

        }
        
        public void RequestInternal(int idx, Response response)
        {
            
    int t = 5;
    response.SetLayoutModel<_layout>((md) =>
    {
        md.Title = "AAAAAAAAAAA";
        return md;
    });
    response.Model.Index = 3;

        }

        public override void Request(Request request,Response response)
        {
            object[] parms=request.Body.Paramters;
            RequestInternal((int)parms[0],response);
        }
        
        public static object[] Parameters(int p0)
        {
            return new object[] {p0 };
        }

    }
}
