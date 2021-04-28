using System;
using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using PluginSystem.API;

namespace EditorExtension
{
    public class BackColorChange : SuperMemoExtension
    {
        private static Color m_PreviousColor = Color.Red;

        public override int Order
        {
            get { return 3; }
        }

        public override string Name
        {
            get
            {
                return @"&Formatowanie tekstu\Ustaw &kolor t³a...";
            }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.Y;
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override void Execute()
        {
            using (var color = new ColorDialog())
            {
                color.Color = m_PreviousColor;

                if (color.ShowDialog() == DialogResult.OK)
                {
                    string colorValue = string.Format("rgb({0}, {1}, {2})", color.Color.R, color.Color.G, color.Color.B);
                    string style = string.Format("<span style=\"background-color: {0}\">{1}</span>", colorValue, SuperMemo.GetSelectedText());
                    SuperMemo.ReplaceSelectedTextWithHtml(style);
                    m_PreviousColor = color.Color;                    
                }
            }
        }
    }
}