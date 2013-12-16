using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public class SectionInfo
    {
        public Response Response { get; set; }
        public Action<StringBuilder, Response, TimSection> Section { get; set; }
        public Type DefineType { get; set; }
        public Dictionary<string, SectionInfo> OwnerSections { get; set; }
    }
    public class TimSection
    {
        public StringBuilder Html { get; set; }
        public SectionInfo MainInvoke { get; set; }
        public void Invoke(Type callFrom)
        {
            MainInvoke.Section(Html, MainInvoke.Response, this);
        }
        public void Invoke(Type callFrom, string name)
        {
            SectionInfo section = null;
            if (MainInvoke.OwnerSections.TryGetValue(name, out section)
                && callFrom == section.DefineType)
            {
                var secInvoke = new TimSection { Html = Html, MainInvoke = section, };
                section.Section(Html, section.Response, secInvoke);
            }
        }
    }
}
