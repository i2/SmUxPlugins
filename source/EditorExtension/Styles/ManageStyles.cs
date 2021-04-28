using System.Collections.Generic;
using System.Windows.Forms;
using PluginSystem.API;
using PluginSystem.Common;
using PluginSystem.Configuration;
using PluginSystem.Plugin;

namespace EditorExtension.Styles
{
    public class ManageStyles : SuperMemoExtension<EditorExtensionsConfiguration>
    {
        public static StyleCollection Styles
        {
            get
            {
                return Configuration.Styles;
            }
        }
        
        public override int Order
        {
            get { return 0; }
        }

        public override string Name
        {
            get
            {
                return @"&Style\Zarz¹dzaj stylami...";
            }
        }

        public override bool Enabled
        {
            get { return CurrentState.IsInEditMode; }
        }

        private static int m_LastSelectedStyle = 0;

        private static void ReformatItems(IList<Style> styles, IList<Style> list)
        {
            //TO DO
        }

        public static void ApplyStyle()
        {
            if (Styles.Count == 0)
            {
                MessageBox.Show("Nie zdefiniowano ¿adnych styli. Dodaj nowy styl w opcji konfiguracji");
            }

            string selectedHtml = SuperMemo.GetSelectedHtml();
            if (!string.IsNullOrEmpty(selectedHtml))
            {
                var style = DlgSelectItem<Style>.SelectItem("Wybór stylu", "Wybierz styl", Styles, m_LastSelectedStyle);
                if (style != null)
                {
                    m_LastSelectedStyle = Styles.IndexOf(style);
                    SuperMemo.ReplaceSelectedTextWithHtml(style.ToHtml(selectedHtml));
                }
            }
        }
 
        public override void Execute()
        {
            using (var styleEditor = new CollectionEditor<Style>(Styles, true))
            {
                if (styleEditor.ShowDialog() == DialogResult.OK)
                {
                    ReformatItems(Styles, styleEditor.Result);
                    Styles.Clear();
                    Styles.AddRange(styleEditor.Result);
                    ConfigurationManager.SaveConfigurations();
                }
            }
        }
    }
}