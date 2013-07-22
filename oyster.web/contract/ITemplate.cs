using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace oyster.web
{
    public interface ITemplate
    {
        Request Init(HttpContext context);
        void Request(Request request, Response response);
        StringBuilder Rander(dynamic Model);
    }
}
