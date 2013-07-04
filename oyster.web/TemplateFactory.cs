using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace oyster.web
{
    public class TemplateFactory
    {
        static Dictionary<Type, ITemplate> instanceDic = new Dictionary<Type, ITemplate>();
        static object lockHandle = new object();


        static Dictionary<Assembly, ISetting> settingDic = new Dictionary<Assembly, ISetting>();
        static object lockSetHandle = new object();

        public static ITemplate GetTemplateInstance(Type tp)
        {
            ITemplate t = null;
            if (!instanceDic.TryGetValue(tp, out t))
            {
                lock (lockHandle)
                {
                    if (!instanceDic.TryGetValue(tp, out t))
                    {
                        t = Activator.CreateInstance(tp) as ITemplate;
                        if (t != null)
                            instanceDic.Add(tp, t);
                        else
                            throw new Exception(string.Format("{0} is not ITemplate!", tp.FullName));
                    }
                }
            }
            return t;
        }

        public static ISetting GetTemplateSetting(Type tpTemp)
        {
            ISetting set = null;
            var asmb = tpTemp.Assembly;
            if (!settingDic.TryGetValue(asmb, out set))
            {
                lock (lockSetHandle)
                {
                    if (!settingDic.TryGetValue(asmb, out set))
                    {
                        var tps = asmb.GetTypes();
                        foreach (var tp in tps)
                        {
                            if (typeof(ISetting).IsAssignableFrom(tp))
                            {
                                set = Activator.CreateInstance(tp) as ISetting;
                                if (set != null)
                                {
                                    settingDic.Add(asmb, set);
                                }
                            }
                        }
                    }
                }
            }
            return set;
        }
    }
}
