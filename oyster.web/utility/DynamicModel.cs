using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Dynamic;
using System.Collections;
using System.Runtime.Serialization;

namespace oyster.web
{
    [Serializable]
    public class TimDynamicModel : DynamicObject, ISerializable
    {
        public TimDynamicModel()
        {
        }
        KeyValueCollection<string, object> dataSet = new KeyValueCollection<string, object>();
        public override bool TryGetMember(GetMemberBinder binder, out object result)
        {
            object d = null;
            dataSet.TryGetValue(binder.Name, out d);
            if (d == null && binder.Name == "Head")
            {
                d = new TimDynamicModel();
                dataSet[binder.Name] = d;
            }
            result = d;
            return true;
        }
        public override bool TrySetMember(SetMemberBinder binder, object value)
        {
            dataSet[binder.Name] = value;
            return true;
        }


        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("dataSet", dataSet, typeof(KeyValueCollection<string, object>));
        }
        public TimDynamicModel(SerializationInfo info, StreamingContext context)
            : base()
        {
            dataSet = info.GetValue("dataSet", typeof(KeyValueCollection<string, object>)) as KeyValueCollection<string, object>;
            dataSet = dataSet ?? new KeyValueCollection<string, object>();
        }
    }
}
