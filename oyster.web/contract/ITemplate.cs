using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace oyster.web
{
    public interface ITemplate
    {
        RequestInfo Request();
        StringBuilder RanderTemplate(DTContainer container);
    }
}
