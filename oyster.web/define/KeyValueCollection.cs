using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace oyster.web.define
{
    [Serializable]
    public class KeyValueCollection<TKey, TValue> : List<KeyValuePair<TKey, TValue>>
    {
        List<TKey> saveKeys { get; set; }
        List<TValue> saveValues { get; set; }
        object lockObject = new object();

        public KeyValueCollection()
        {
            saveKeys = new List<TKey>();
            saveValues = new List<TValue>();
        }


        private void ChangeData(KeyValuePair<TKey, TValue> e, int op)
        {
            lock (lockObject)
            {
                switch (op)
                {
                    case -1:
                        base.Remove(e);
                        saveKeys.Remove(e.Key);
                        saveValues.Remove(e.Value);
                        break;
                    case 0:
                        base.Add(e);
                        saveKeys.Add(e.Key);
                        saveValues.Add(e.Value);
                        break;
                    case 1:
                        for (int i = 0; i < base.Count; i++)
                        {
                            var be = base[i];
                            if (be.Key.Equals(e.Key))
                            {
                                saveValues.Remove(be.Value);
                                saveValues.Add(e.Value);
                                base[i] = e;
                                break;
                            }
                        }
                        break;
                }
            }
        }


        public void Add(TKey key, TValue value)
        {
            if (!ContainsKey(key))
            {
                ChangeData(new KeyValuePair<TKey, TValue>(key, value), 0);
            }
            else
                throw new Exception("Have same key in Collection!");
        }

        public bool ContainsKey(TKey key)
        {
            foreach (var k in saveKeys)
            {
                if (k.Equals(key))
                    return true;
            }
            return false;
        }

        public ICollection<TKey> Keys
        {
            get { return saveKeys; }
        }

        public bool Remove(TKey key)
        {
            if (key == null)
                return false;

            for (int i = 0; i < Count; i++)
            {
                var e = base[i];
                if (key.Equals(e.Key))
                {
                    ChangeData(e, -1);
                    return true;
                }
            }
            return false;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            value = default(TValue);
            if (key == null)
                return false;

            for (int i = 0; i < Count; i++)
            {
                var e = base[i];
                if (key.Equals(e.Key))
                {
                    value = e.Value;
                    return true;
                }
            }
            return false;
        }

        public ICollection<TValue> Values
        {
            get { return saveValues; }
        }

        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    return default(TValue);
                for (int i = 0; i < Count; i++)
                {
                    var e = base[i];
                    if (key.Equals(e.Key))
                    {
                        return e.Value;
                    }
                }
                return default(TValue);
            }
            set
            {
                if (key == null)
                    return;
                var kv = new KeyValuePair<TKey, TValue>(key, value);
                for (int i = 0; i < Count; i++)
                {
                    var e = base[i];
                    if (key.Equals(e.Key))
                    {
                        ChangeData(kv, 1);
                        return;
                    }
                }
                ChangeData(kv, 0);
            }
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ChangeData(item, 0);
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue d = default(TValue);
            if (TryGetValue(item.Key, out d) &&
                (d != null && d.Equals(item.Value) || d == null && item.Value == null))
                return true;
            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            for (int i = 0; i < Count; i++)
            {
                array[arrayIndex + i] = base[i];
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }
    }
}
