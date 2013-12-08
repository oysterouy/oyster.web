using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web.hosting
{
    public class InvorkInfo
    {
        public Response Response { get; set; }
        public Action<StringBuilder, Response, SectionInvork> Section { get; set; }
        public Type DefineType { get; set; }
        public Dictionary<string, InvorkInfo> OwnerSections { get; set; }
    }
    public class SectionInvork
    {
        public StringBuilder Html { get; set; }
        public InvorkInfo MainInvoke { get; set; }
        public void Invoke(Type callFrom)
        {
            MainInvoke.Section(Html, MainInvoke.Response, this);
        }
        public void Invoke(Type callFrom, string name)
        {
            InvorkInfo section = null;
            if (MainInvoke.OwnerSections.TryGetValue(name, out section)
                && callFrom == section.DefineType)
            {
                var secInvoke = new SectionInvork { Html = Html, MainInvoke = section, };
                section.Section(Html, section.Response, secInvoke);
            }
        }
    }
}
