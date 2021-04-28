using System.Drawing;
using System.Windows.Forms;
using PluginSystem.Plugin;
using System.IO;
using PluginSystem.Helpers;
using System.Diagnostics;
using mshtml;
using PluginSystem.API;

namespace EditorExtension
{
    public class ColorChange : SuperMemoExtension
    {
        private static Color m_PreviousColor = Color.Red;

        public override int Order
        {
            get { return 2; }
        }

        public override string Name
        {
            get
            {
                return @"&Formatowanie tekstu\Ustaw &kolor...";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0; }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Shift | Keys.T;
        }

        public override void Execute()
        {
            using (var color = new ColorDialog())
            {
                color.Color = m_PreviousColor;

                if (color.ShowDialog() == DialogResult.OK)
                {
                    m_PreviousColor = color.Color;
                    string colorValue = string.Format("rgb({0}, {1}, {2})", color.Color.R, color.Color.G, color.Color.B); 
                    SuperMemo.ExecCommand("ForeColor", false, colorValue);
                }
            }
        }
    }
}