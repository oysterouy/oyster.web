﻿using System;
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
                        var tpHost = typeof(HostBase);
                        foreach (var tp in tps)
                        {
                            if (typeof(HostBase).IsAssignableFrom(tp))
                            {
                                var obj = Activator.CreateInstance(tp);
                                setting = obj as HostBase;
                                if (setting != null)
                                    templateAssemblySettings.Add(asm.FullName, setting);
                                break;
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
        static readonly Dictionary<Type, TemplateBase> templateInstances = new Dictionary<Type, TemplateBase>();
        static object lockTemplate = new object();
        public static TemplateBase GetTemplateInstance(Type tempTp)
        {
            TemplateBase temp = null;
            if (!templateInstances.TryGetValue(tempTp, out temp))
            {
                lock (lockTemplate)
                {
                    if (!templateInstances.TryGetValue(tempTp, out temp))
                    {
                        if (typeof(TemplateBase).IsAssignableFrom(tempTp))
                        {
                            temp = Activator.CreateInstance(tempTp) as TemplateBase;
                            if (temp != null)
                                templateInstances.Add(tempTp, temp);
                        }
                        else
                        {
                            throw new Exception(string.Format("{0} is not a TemplateBase Type!", tempTp.FullName));
                        }
                    }
                }
            }

            return temp;
        }
      
    }
}
