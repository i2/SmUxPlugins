using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using AxSHDocVw;
using PluginSystem.API;
using PluginSystem.Plugin;

namespace EditorExtension
{
    public class ToggleIPAPanel : SuperMemoExtension<EditorExtensionsConfiguration>
    {
        private Panel m_IPAPanel;

        public override int Order
        {
            get { return 101; }
        }

        public override string Name
        {
            get
            {
                return @"Inne\&Pokaż/ukryj tabelę znaków IPA";
            }
        }

        public override bool Enabled
        {
            get { return SuperMemo.VisualizerMode == 0 | SuperMemo.VisualizerMode == 1 | (m_IPAPanel != null && m_IPAPanel.Visible); }
        }

        public override Keys GetShortcut()
        {
            return Keys.Control | Keys.Alt | Keys.I;
        }

        public override void Execute()
        {
            if (m_IPAPanel == null)
            {
                InstallIPAPanel();
            }
            else
            {
                m_IPAPanel.Visible = !m_IPAPanel.Visible;
            }
        }

        private void InstallIPAPanel()
        {
            m_IPAPanel = new Panel();
            m_IPAPanel.BorderStyle = BorderStyle.FixedSingle;
            AxWebBrowser browser = SuperMemo.GetWebBrowser();
            browser.Parent.Controls.Add(m_IPAPanel);
            PositionPanel();
            browser.SizeChanged += BrowserOnSizeChanged;
            browser.Move += BrowserOnMove;
            m_IPAPanel.BackColor = SystemColors.Control;
            
            var flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Fill;
            flowLayoutPanel.Padding = new Padding(6);
            m_IPAPanel.Controls.Add(flowLayoutPanel);

            var tooltip = new ToolTip();
            tooltip.StripAmpersands = true;

            IList<IPASymbol> ipaSymbols = Configuration.IPASymbols;
            
            foreach (var ipaLetter in ipaSymbols)
            {
                var label = new Label();
                label.Font = new Font("Arial", 12);
                label.Text = ipaLetter.Symbol;
                label.AutoSize = true;
                label.BorderStyle = BorderStyle.FixedSingle;
                label.Click += LabelOnClick;

                if (ipaLetter.Type != "vowel")
                {
                    label.BackColor = SystemColors.ControlLight;
                }
                else
                {
                    label.BackColor = SystemColors.ControlLightLight;
                }
                label.Margin = new Padding(5);

                tooltip.SetToolTip(label, ipaLetter.GetToolTipText());
                flowLayoutPanel.Controls.Add(label);
            }
        }

        private static void LabelOnClick(object sender, EventArgs args)
        {
            var label = sender as Label;
            SendKeys.Send(label.Text);
        }

        private void BrowserOnMove(object sender, EventArgs args)
        {
            PositionPanel();            
        }

        private void BrowserOnSizeChanged(object sender, EventArgs args)
        {
            PositionPanel();
        }

        private void PositionPanel()
        {
            AxWebBrowser browser = SuperMemo.GetWebBrowser();
            m_IPAPanel.SetBounds(browser.Left, browser.Bottom, browser.Width, 70);
        }
    }
}