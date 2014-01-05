using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

namespace oyster.web
{
    public abstract class TimTemplate : TimTemplateBase
    {
        protected static readonly
            ConcurrentDictionary<Type, Dictionary<string, Action<StringBuilder, Response, TimSection>>>
            allTemplateSections = new ConcurrentDictionary<Type, Dictionary<string, Action<StringBuilder, Response, TimSection>>>();

        Dictionary<string, Action<StringBuilder, Response, TimSection>> templateSections
        {
            get
            {
                Dictionary<string, Action<StringBuilder, Response, TimSection>> val = null;

                if (!allTemplateSections.TryGetValue(this.GetType(), out val))
                {
                    val = new Dictionary<string, Action<StringBuilder, Response, TimSection>>();
                    allTemplateSections.TryAdd(this.GetType(), val);
                }
                return val;
            }
        }

        public abstract object[] Init(Request request);

        internal void Init(TimProcess timProcess, params object[] args)
        {
            TimProcessContext.SetProcess(timProcess);
            if (args != null && args.Length > 0)
                timProcess.Response.Paramters = args;
            else
                timProcess.Response.Paramters = Init(timProcess.Request);

            timProcess.InitMainBlock();
        }

        public abstract void Request(TimProcess timProcess);

        internal void Render(TimProcess timProcess, Dictionary<string, SectionInfo> action = null)
        {
            var actDic = new Dictionary<string, SectionInfo>();
            foreach (var actKv in templateSections)
            {
                actDic.Add(actKv.Key, new SectionInfo
                {
                    Response = timProcess.Response,
                    DefineType = this.GetType(),
                    Section = actKv.Value,
                    OwnerSections = actDic,
                });
            }
            if (action != null)
            {
                foreach (var kv in action)
                {
                    if (kv.Key == "Body")
                        continue;

                    var actKv = kv;
                    if (actKv.Key == "Page")
                    {
                        actKv = new KeyValuePair<string, SectionInfo>("Body", kv.Value);
                    }
                    if (actDic.ContainsKey(actKv.Key))
                    {
                        //layout 也定义了则最先定义的Type为layout
                        actKv.Value.DefineType = this.GetType();
                        actDic[actKv.Key] = actKv.Value;
                    }
                    else
                        actDic.Add(actKv.Key, actKv.Value);
                }
            }
            if (timProcess.Layout != null && timProcess.Layout.Template != null)
            {
                timProcess.Layout.Template.Render(timProcess.Layout, actDic);
            }
            else
            {
                SectionInfo pageSection = null;
                if (actDic.TryGetValue("Page", out pageSection))
                {
                    var invorker = new TimSection { Html = timProcess.Response.Body, MainInvoke = pageSection };
                    invorker.Invoke(this.GetType());
                }
            }


        }
    }
    public abstract class TimTemplate<T> : TimTemplate
    where T : TimTemplate
    {
        public static T Instance { get { return InstanceHelper<T>.Instance; } }
        protected static Dictionary<string, Action<StringBuilder, Response, TimSection>> TemplateSections
        {
            get
            {
                Dictionary<string, Action<StringBuilder, Response, TimSection>> val = null;
                if (!allTemplateSections.TryGetValue(typeof(T), out val))
                {
                    val = new Dictionary<string, Action<StringBuilder, Response, TimSection>>();
                    allTemplateSections.TryAdd(typeof(T), val);
                }
                return val;
            }
        }
    }
}
