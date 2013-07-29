
namespace demotheme
{
    using oyster.web;
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
</div>
");
            invorker.Invork("Foot");});
templateSections.Add("Foot",(html,response,invorker)=>{
    dynamic Model=response.Model;

            Echo(html, @"<div>
        <h2>
            我是继承页页脚</h2>
    </div>
");});

        }

        
        public override dynamic Init(Request request)
        {
            
    request.Layout<_layout>();
    return request.Body = new RequestBody { Model = new { A = 1, B = 3 } };

        }
        
        public override void Request(Request request,Response  response)
        {
            
    int t = 5;
    response.SetLayoutModel<_layout>((md) =>
    {
        md.Title = "AAAAAAAAAAA";
        return md;
    });
    response.Model = new { Index = request.Body.Model.A + request.Body.Model.B * 13 };

        }

    }
}
