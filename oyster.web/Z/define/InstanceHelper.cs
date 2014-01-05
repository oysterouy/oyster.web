using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace oyster.web
{
    class Box<T>
    {

    }
    public class InstanceHelper
    {
        protected static Dictionary<string, object> _instances = new Dictionary<string, object>();

        public static object GetInstance(Type tname, params object[] paramsobj)
        {
            object t = null;
            var boxTp = typeof(Box<>).MakeGenericType(tname);
            lock (boxTp)
            {
                string key = tname.FullName;
                if (paramsobj != null)
                {
                    foreach (object obj in paramsobj)
                    {
                        if (obj != null)
                        {
                            key += string.Format("-[{0}:{1}]", obj.GetType().FullName, obj.GetHashCode().ToString());
                        }
                    }
                }

                if (!_instances.ContainsKey(key))
                {
                    t = Activator.CreateInstance(tname, paramsobj);
                    if (!_instances.ContainsKey(key))
                        _instances.Add(key, t);
                }
                else
                {
                    t = _instances[key];
                }
            }
            return t;
        }
    }

    public class InstanceHelper<T> : InstanceHelper
    {
        public static T Instance
        {
            get
            {
                return (T)GetInstance(typeof(T));
            }
        }

        public static T GetInstance(object[] paramsobj = null)
        {
            return (T)GetInstance(typeof(T), paramsobj);
        }
    }
}
