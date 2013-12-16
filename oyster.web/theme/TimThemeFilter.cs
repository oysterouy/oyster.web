using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using oyster.web.manage;
using System.Collections.Concurrent;

namespace oyster.web
{
    public abstract partial class TimTheme
    {
        internal bool BeforeRouteFilter(TimProcess process)
        {
            return RunningFilter(process, FilterType.BeforeRoute);
        }
        internal bool BeforeRequestFilter(TimProcess process)
        {
            return RunningFilter(process, FilterType.BeforeRequest);
        }
        internal bool BeforeRenderFilter(TimProcess process)
        {
            return RunningFilter(process, FilterType.BeforeRender);
        }
        internal bool AfterRenderFilter(TimProcess process)
        {
            return RunningFilter(process, FilterType.AfterRender);
        }
        bool RunningFilter(TimProcess process, FilterType type)
        {
            ConcurrentDictionary<FilterType, List<Func<TimProcess, bool>>> thisFilterFuncs = null;
            List<Func<TimProcess, bool>> funcLs = null;
            if (filters.TryGetValue(this, out thisFilterFuncs)
                && thisFilterFuncs.TryGetValue(type, out funcLs))
            {
                foreach (var func in funcLs)
                {
                    //返回false 表示不再继续
                    if (func != null && !func(process))
                        return false;
                }
            }

            return true;
        }

        public abstract int LoadingTimeout { get; }
        public abstract string ThemeRelactivePath { get; }

        protected enum FilterType
        {
            BeforeRoute = 0,
            BeforeRequest = 1,
            BeforeRender = 2,
            AfterRender = 3,
        }
        static readonly ConcurrentDictionary<TimTheme, ConcurrentDictionary<FilterType, List<Func<TimProcess, bool>>>> filters
              = new ConcurrentDictionary<TimTheme, ConcurrentDictionary<FilterType, List<Func<TimProcess, bool>>>>();
        protected virtual void AddFilter(Func<TimProcess, bool> filterFunc, FilterType type)
        {
            ConcurrentDictionary<FilterType, List<Func<TimProcess, bool>>> thisFilterFuncs
                = filters.GetOrAdd(this, new ConcurrentDictionary<FilterType, List<Func<TimProcess, bool>>>());

            var ls = thisFilterFuncs.GetOrAdd(type, new List<Func<TimProcess, bool>>());

            ls.Add(filterFunc);
        }
        protected static void AddFilter<TTheme>(Func<TimProcess, bool> filterFunc, FilterType type)
            where TTheme : TimTheme
        {
            InstanceHelper<TTheme>.Instance.AddFilter(filterFunc, type);
        }
    }
}
