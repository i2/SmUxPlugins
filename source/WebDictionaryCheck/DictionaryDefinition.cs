using System;
using System.ComponentModel;

namespace WebDictionaries
{
    [Serializable]
    public class DictionaryDefinition : ICloneable
    {
        [DisplayName("Nazwa")]
        [Description("Nazwa słownika")]
        public string Name
        {
            get; set;
        }

        [DisplayName("Polecenie")]
        [Description("Polecenie wykonywane w przeglądarce zewnętrznej. Znacznik {0} oznacza zaznaczony w momencie wywołania tekst")]
        public string Command
        {
            get; set;
        }

        public override string ToString()
        {
            return Name;
        }

        #region ICloneable Members

        public object Clone()
        {
            var dictionaryDefinition = new DictionaryDefinition {Name = Name, Command = Command};
            return dictionaryDefinition;
        }

        #endregion
    }
}