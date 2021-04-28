using System;

namespace PluginSystem.Common
{
    public class ValueRestorer<TObjectType, TPropertyValue> : IDisposable
    {
        private readonly TObjectType m_Object;
        private readonly TPropertyValue m_OriginalValue;
        private readonly Action<TObjectType, TPropertyValue> m_Setter;

        public ValueRestorer(TObjectType @object, Action<TObjectType, TPropertyValue> setter, Func<TObjectType, TPropertyValue> getter, TPropertyValue temporaryValue):this(@object, setter, getter)
        {            
            m_Setter(m_Object, temporaryValue);
        }

        public ValueRestorer(TObjectType @object, Action<TObjectType, TPropertyValue> setter, Func<TObjectType, TPropertyValue> getter)
        {
            m_Object = @object;
            m_Setter = setter;
            m_OriginalValue = getter(m_Object);
        }

        public ValueRestorer(TObjectType @object, Action<TObjectType, TPropertyValue> setter, TPropertyValue originalValue)
        {
            m_Object = @object;
            m_Setter = setter;

            m_OriginalValue = originalValue;
        }

        public void Dispose()
        {
            m_Setter(m_Object, m_OriginalValue);
        }
    }
}
