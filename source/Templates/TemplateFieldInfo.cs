using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Templates
{
    public class TemplateFieldInfo
    {
        private readonly Dictionary<string, object> m_Variables = new Dictionary<string, object>();
        private readonly string m_OriginalTag;

        public Dictionary<string, object> Variables
        {
            get { return m_Variables; }
        }

        public bool IsField
        {
            get
            {
                return m_Variables.ContainsKey("Field") && m_Variables["Field"] is string;
            }
        }

        public bool IsBlockBegin
        {
            get
            {
                return m_Variables.ContainsKey("BlockBegin") && m_Variables["BlockBegin"] is string;
            }
        }

        public bool IsBlockEnd
        {
            get
            {
                return m_Variables.ContainsKey("BlockEnd") && m_Variables["BlockEnd"] is string;
            }
        }

        public bool IsBlock
        {
            get
            {
                return IsBlockBegin || IsBlockEnd;
            }
        }

        private bool IsRequired
        {
            get { return Variables.ContainsKey("Required"); }
        }

        private bool IsMultiline
        {
            get { return Variables.ContainsKey("MultiLine"); }
        }

        private bool HasHistory
        {
            get { return Variables.ContainsKey("History"); }
        }

        public string OriginalTag
        {
            get { return m_OriginalTag; }
        }

        public TemplateFieldInfo(string tag)
        {
            m_OriginalTag = tag;
            tag = tag.Substring(1, tag.Length - 2);
            string[] keyValueParis = tag.Split(',');

            foreach (string keyValue in keyValueParis)
            {
                if (keyValue.Contains("="))
                {
                    string[] pair = keyValue.Split(new[] {'='}, 2);

                    string key = pair[0].Trim();
                    string valueAsString = pair[1].Trim();

                    object value;

                    if (valueAsString.StartsWith("\"") && valueAsString.EndsWith("\""))
                    {
                        value = valueAsString.Substring(1, valueAsString.Length - 2);
                    }
                    else 
                    {
                        bool valueRecognised;

                        value = bool.TryParse(valueAsString, out valueRecognised);

                        if (!valueRecognised)
                        {
                            value = valueAsString;
                        }
                    }
                    m_Variables[key] = value;
                }
                else
                {
                    m_Variables[keyValue.Trim()] = null;
                }
            }
        }

        public string GetBlockBeginName()
        {
            return (string)m_Variables["BlockBegin"];
        }

        public string GetBlockEndName()
        {
            return (string)m_Variables["BlockEnd"];
        }

        public string FieldName
        {
            get { return (string) m_Variables["Field"]; }
        }

        public Control GetFieldLabel()
        {
            var label = new Label
                            {
                                Text = "&" + FieldName,
                                Dock = DockStyle.Fill,
                                TextAlign = ContentAlignment.MiddleLeft
                            };
            //label.Top = 10;

            return label;
        }

        public Control GetFieldEditor()
        {
            Control control = HasHistory ? (Control) new ComboBox(): new TextBox();
            control.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;

            var textBox = control as TextBox;
            var comboBox = control as ComboBox;

            if (textBox != null)
            {
                //textBox.Text = GetFieldName();
                textBox.SelectionStart = 0;
                textBox.SelectionLength = textBox.Text.Length;

                if (IsMultiline)
                {
                    textBox.Multiline = true;
                    textBox.Height = textBox.Height*4;
                }
            }

            if (IsRequired)
            {
                control.Validating += RequiredNotEmptyValue;
            }

            if (comboBox != null)
            {
                if (AddTemplateBasedItem.FieldsHistory.ContainsKey(FieldName))
                {
                    List<string> historicalFieldValues = AddTemplateBasedItem.FieldsHistory[FieldName];

                    if (historicalFieldValues != null)
                    {
                        foreach (string historicalFieldValue in historicalFieldValues)
                        {
                            comboBox.Items.Add(historicalFieldValue);
                        }
                    }
                }

                if (comboBox.Items.Count > 0)
                {
                    comboBox.SelectedIndex = 0;
                }

                comboBox.SelectAll();
            }

            return control;
        }

        public Control GetFieldButton()
        {
            var label2 = new Label();
            return label2;
        }

        private static void RequiredNotEmptyValue(object sender, CancelEventArgs e)
        {
            var textBox = (TextBox)sender;
            var dlgAddTemplateItem = textBox.FindForm() as DlgAddTemplateItem;

            if (dlgAddTemplateItem != null)
            {
                if (string.IsNullOrEmpty(textBox.Text))
                {
                    dlgAddTemplateItem.errorProvider1.SetError(textBox, "Pole musi byæ wype³nione");
                }
                else
                {
                    dlgAddTemplateItem.errorProvider1.SetError(textBox, null);
                }
            }
        }
    }
}