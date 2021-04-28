using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace PluginSystem.Common
{
    [Serializable]
    public class SerializableList<T> : List<T>, ISerializable
    {
        #region Constants

        private const string COUNT_LABEL = "Count";
        private const string ITEM_LABEL = "Item";

        #endregion
        #region Ctors

        public SerializableList()
        {
        }

        protected SerializableList(SerializationInfo info, StreamingContext context)
        {
            int count = info.GetInt32(COUNT_LABEL);
            int i = 0;
            while (count > 0)
            {
                T t = (T)info.GetValue(ITEM_LABEL + i, typeof(T));
                Add(t);
                count--;
                i++;
            }
        }

        #endregion
        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(COUNT_LABEL, Count);
            int i = 0;
            foreach (T t in this)
            {
                info.AddValue(ITEM_LABEL + i, t, t.GetType());
                i++;
            }
        }

        #endregion
    }
}