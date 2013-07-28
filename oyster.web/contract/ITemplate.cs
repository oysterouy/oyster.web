using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;


namespace oyster.web
{
    public interface ITemplate
    {
        dynamic Init(Request request);
        void Request(Request request, Response response);
        StringBuilder Rander(dynamic Model);
    }
}
