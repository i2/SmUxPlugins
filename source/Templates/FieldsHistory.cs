using System;
using System.Collections;
using System.Xml;
using System.Xml.Schema;
using PluginSystem.Common;

namespace Templates
{
    [Serializable]
    public class FieldsHistory : SerializableDictionary<string, SerializableList<string>>
    {
    }
}