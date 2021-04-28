using System;
using PluginSystem.Common;

namespace EditorExtension.Styles
{
    [Serializable]
    public class StyleCollection : SerializableList<Style>
    {
        public Style GetStyleByName(string name)
        {
            return Find(style => style.Name == name);
        }
    }
}