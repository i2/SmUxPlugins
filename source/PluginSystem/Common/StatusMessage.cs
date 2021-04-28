using System;
using System.Drawing;
using System.Media;
using System.Windows.Forms;
using PluginSystem.API;

namespace PluginSystem.Common
{
    public static class StatusMessage
    {
        private static Label m_StatusMessageLabel;
        private static bool m_MessageFilterInstalled;
        private static readonly MessageFilter m_MessageFilter = new MessageFilter();
        private static ToolTip m_ToolTip = new ToolTip();

        public static void ShowError(string message, Exception ex)
        {
            Show(MessageType.Error, message, "Wyj¹tek: \n" + ex);
        }

        public static void Show(MessageType messageType, string message)
        {
            Show(messageType, message, null);            
        }

        public static void Show(MessageType messageType, string message, string details)
        {
            if (!String.IsNullOrEmpty(message))
            {
                InstallMessageFilter();
                if (m_StatusMessageLabel == null)
                {
                    m_StatusMessageLabel = new Label
                                               {
                                                   Dock = DockStyle.Bottom,
                                                   TextAlign = ContentAlignment.MiddleCenter,
                                                   BorderStyle = BorderStyle.FixedSingle,
                                                   BackColor = Color.Azure,
                                                   Font = new Font(SuperMemo.MainForm.Font, FontStyle.Bold)
                                               };
                }

                m_StatusMessageLabel.ForeColor = GetMessageColor(messageType);
                m_StatusMessageLabel.Text = message;
                m_StatusMessageLabel.AutoEllipsis = true;
                m_ToolTip.SetToolTip(m_StatusMessageLabel, details);

                SuperMemo.MainForm.Controls.Add(m_StatusMessageLabel);
                Application.DoEvents();

                if (messageType != MessageType.Info)
                {
                    SystemSounds.Beep.Play();
                }
            }
            else
            {
                Remove();
            }
        }

        private static Color GetMessageColor(MessageType type)
        {
            switch (type)
            {
                case MessageType.Info:
                    return Color.Black;
                case MessageType.Warning:
                    return Color.Yellow;
                case MessageType.Error:
                    return Color.Red;
                default:
                    throw new ArgumentOutOfRangeException("type");
            }
        }

        internal static void InstallMessageFilter()
        {
            if (!m_MessageFilterInstalled)
            {
                Application.RemoveMessageFilter((IMessageFilter)SuperMemo.MainForm);
                Application.AddMessageFilter(m_MessageFilter);
                Application.AddMessageFilter((IMessageFilter)SuperMemo.MainForm);
                m_MessageFilterInstalled = true;
            }
        }

        public static void Remove()
        {
            if (m_StatusMessageLabel != null)
            {
                m_StatusMessageLabel.Dispose();
                m_StatusMessageLabel = null;
                Application.DoEvents();
            }
        }

        public static void ShowInfo(string statusMessage)
        {
            Show(MessageType.Info, statusMessage);
        }
    }
}