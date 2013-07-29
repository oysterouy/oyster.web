using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public class InvorkInfo
    {
        public Response Response { get; set; }
        public Action<StringBuilder, Response, SectionInvork> Section { get; set; }
    }
    public class SectionInvork
    {
        public StringBuilder Html { get; set; }
        public Dictionary<string, InvorkInfo> Sections { get; set; }
        public void Invork(string name = "Page")
        {
            InvorkInfo section = null;
            if (!Sections.TryGetValue(name, out section))
            {
                if (name == "Page")
                    throw new Exception("Page Section No Found!");
                else
                    return;
            }

            section.Section(Html, section.Response, this);
        }
    }
}
