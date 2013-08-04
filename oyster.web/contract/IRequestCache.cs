using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public interface IRequestCache
    {
        string GetResponseText(Func<object[], string> keySelect, Func<string> cacheSelect);
        bool PutResponseText(Func<object[], string> keySelect, Func<string> cacheSelect);
    }
}
