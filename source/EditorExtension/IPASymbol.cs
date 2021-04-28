using System;
using System.ComponentModel;

namespace EditorExtension
{
    [Serializable]
    public class IPASymbol
    {
        [DisplayName("Symbol")]
        [Description("Symbol, który ma byæ szybko dostêpny w postaci przycisku")]
        public string Symbol
        {
            get; set;
        }

        [DisplayName("Typ symbolu")]
        [Description("Typ symbolu, np. consonant, vowel")]
        public string Type
        {
            get;
            set;
        }

        [DisplayName("Przyk³ad")]
        [Description("Przyk³ad prostego s³owa zawieraj¹cego dany symbol")]
        public string Example
        {
            get;
            set;
        }

        public IPASymbol()
        {
        }

        public IPASymbol(string symbol, string example, string type)
        {
            Symbol = symbol;
            Type = type;
            Example = example;
        }

        public string GetToolTipText()
        {
            return string.Format("({0}) \n{1}", Type, Example);
        }

        public override string ToString()
        {
            return string.Format("Symbol: {0}, Type: {1}, Example: {2}", Symbol, Type, Example);
        }
    }
}