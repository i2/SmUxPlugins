using System;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Drawing;
using PluginSystem.Helpers;

namespace EditorExtension.Styles
{
    [Serializable]
    public class Style : ICloneable
    {
        [Description("Nazwa stylu, pod jaką będzie on wyświetlany")]
        [DisplayName("Nazwa")]
        [Browsable(true)]
        [Category("Ogólne")]
        public string Name
        {
            get;set;
        }

        [Description("Okresla kolor tekstu")]
        [Category("Kolory")]
        [DisplayName("Kolor")]
        [Browsable(true)]
        public string Color
        {
            get; set;
        }

        [Description("Określa styl czcionki")]
        [Category("Czcionka")]
        [DisplayName("Styl czcionki")]
        [Browsable(true)]
        public string FontStyle
        {
            get;
            set;
        }

        [Browsable(false)]
        public string CssName
        {
            get
            {
                return Name.Replace(" ", string.Empty).Replace("ż","z");
            }
        }

        public string ToHtml(string innerHtml)
        {
            string leftPart = GetHtmlLeftPart();
            string rightPart = GetHtmlRightPart();
            
            return leftPart + innerHtml + rightPart;
        }

        public string GetHtmlRightPart()
        {
            return "</span>";
        }

        public string GetHtmlLeftPart()
        {
            string style = string.Empty;
            
            if (!string.IsNullOrEmpty(FontStyle))
            {
                AddStyleElement(ref style, "font-style", FontStyle);
            }

            if (!string.IsNullOrEmpty(Color))
            {
                AddStyleElement(ref style, "color", Color);
            }

            return string.Format("<span name=\"{0}\" style=\"{1}\">", CssName, style);
        }

        private static void AddStyleElement(ref string style, string elementName, string elementValue)
        {
            if (!string.IsNullOrEmpty(style))
            {
                style += "; ";
            }
            
            style += elementName +  ": " + elementValue;
        }

        public override string ToString()
        {
            if (!string.IsNullOrEmpty(Name))
            {
                return string.Format(Name);
            }

            return "Styl nienazwany";
        }

        #region ICloneable Members

        public object Clone()
        {
            return MemberwiseClone();
        }

        #endregion
    }
}
