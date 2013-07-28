using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace oyster.web
{
    public class TemplateManager
    {
        static readonly Dictionary<string, HostBase> templateAssemblySettings = new Dictionary<string, HostBase>();
        static object lockSetting = new object();
        public static HostBase GetSetting(Assembly asm)
        {
            if (asm == null)
                return null;
            HostBase setting = null;

            if (!templateAssemblySettings.TryGetValue(asm.FullName, out setting))
            {
                lock (lockSetting)
                {
                    if (!templateAssemblySettings.TryGetValue(asm.FullName, out setting))
                    {
                        var tps = asm.GetTypes();
                        foreach (var tp in tps)
                        {
                            if (typeof(HostBase).IsAssignableFrom(tp))
                            {
                                setting = Activator.CreateInstance(tp) as HostBase;
                                if (setting != null)
                                    templateAssemblySettings.Add(asm.FullName, setting);
                            }
                        }
                    }
                }
            }
            return setting;
        }

        internal static HostBase GetSetting(string ProxyModuleName)
        {
            HostBase set = null;
            if (!templateAssemblySettings.TryGetValue(ProxyModuleName, out set))
            {
                var asmls = AppDomain.CurrentDomain.GetAssemblies();
                foreach (var asm in asmls)
                {
                    if (asm.FullName.Equals(ProxyModuleName))
                    {
                        set = GetSetting(asm);
                        if (set != null)
                            break;
                    }
                }
            }
            return set;
        }
        static readonly Dictionary<Type, ITemplate> templateInstances = new Dictionary<Type, ITemplate>();
        static object lockTemplate = new object();
        public static ITemplate GetTemplateInstance(Type tempTp)
        {
            ITemplate temp = null;
            if (!templateInstances.TryGetValue(tempTp, out temp))
            {
                lock (lockTemplate)
                {
                    if (!templateInstances.TryGetValue(tempTp, out temp))
                    {
                        if (typeof(ITemplate).IsAssignableFrom(tempTp))
                        {
                            temp = Activator.CreateInstance(tempTp) as ITemplate;
                            if (temp != null)
                                templateInstances.Add(tempTp, temp);
                        }
                        else
                        {
                            throw new Exception(string.Format("{0} is not a ITemplate Type!", tempTp.FullName));
                        }
                    }
                }
            }

            return temp;
        }

    }
}
