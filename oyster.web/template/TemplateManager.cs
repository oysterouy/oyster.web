using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    public class TemplateManager
    {
        public static ISetting GetSettingFromTemplate(ITemplate temp)
        {
            var asm = temp.GetType().Assembly;
            var tps = asm.GetTypes();
            foreach (var tp in tps)
            {
                if (typeof(ISetting).IsAssignableFrom(tp))
                {
                    return Activator.CreateInstance(tp) as ISetting;
                }
            }
            return null;
        }
    }
}
