using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TWS.Utils
{
    [Serializable]
    public class Dictionary<TKey, TValue> : System.Collections.Generic.Dictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField]
        private List<TKey> keys = new List<TKey>();

        [SerializeField]
        private List<TValue> values = new List<TValue>();

        public Dictionary() : base()
        {}

        public Dictionary(Dictionary<TKey, TValue> source) : base(source)
        {}

        // save the dictionary to lists
        public void OnBeforeSerialize()
        {
            keys.Clear();
            values.Clear();
            foreach (KeyValuePair<TKey, TValue> pair in this)
            {
                keys.Add(pair.Key);
                values.Add(pair.Value);
                //((ISerializationCallbackReceiver)pair.Value).OnBeforeSerialize();
            }
        }

        // load dictionary from lists
        public void OnAfterDeserialize()
        {
            this.Clear();

            if (keys.Count != values.Count)
                throw new System.Exception(string.Format("Es sind {0} Keys und {1} Werte nach der Deserialisierung. Einige Objekte waren nicht serialisierbar."));

            for (int i = 0; i < keys.Count; i++)
            {
                //((ISerializationCallbackReceiver)values[i]).OnAfterDeserialize();
                this.Add(keys[i], values[i]);
            }
        }
    }
}
