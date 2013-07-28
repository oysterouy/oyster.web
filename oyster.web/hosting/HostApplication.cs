using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace oyster.web.hosting
{
    public class HostApplication : MarshalByRefObject
    {
        public HostApplication(string name)
        {
            Name = name;
            HostSetting = new HostProxy();
        }

        public string Name { get; internal set; }
        public HostProxy HostSetting { get; set; }
    }
}
