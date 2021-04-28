using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace PluginSystem.Common
{
    public partial class DlgProgress : BaseForm, IProgress
    {
        public delegate void ProgressActionDelegate(IProgress dlgProgress);

        private bool m_Cancel;
        private readonly ProgressActionDelegate m_ProgressActionDelegate;

        private Exception m_LastException;

        protected Exception LastException
        {
            get 
            {
                return m_LastException;
            }
        }

        public int ProgressValue
        {
            get
            {
                return (int)Invoke((Func<int>) GetProgressValue);
            }
            set 
            {
                Invoke((Action<int>) SetProgressValue, value);
            }
        }

        public string Message
        {
            set
            {
                Invoke((Action<string>)SetMessage, value);
            }
        }

        public bool Cancel
        {
            get { return m_Cancel; }
        }

        public bool Undefined
        {
            get { return (bool) Invoke((Func<bool>)IsProgressUndefined); }
            set { Invoke((Action<bool>)SetProgressUnefined, value); }
        }

        private void SetProgressUnefined(bool value)
        {
            progressBar1.Style = value ? ProgressBarStyle.Marquee : ProgressBarStyle.Continuous;
        }

        private bool IsProgressUndefined()
        {
            return progressBar1.Style == ProgressBarStyle.Marquee;
        }

        private DlgProgress(string title, ProgressActionDelegate progressActionDelegate) : this()
        {
            m_ProgressActionDelegate = progressActionDelegate;
            Text = title;
        }

        public override sealed string Text
        {
            get { return base.Text; }
            set { base.Text = value; }
        }

        private DlgProgress()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            m_Cancel = true;
            btnCancel.Enabled = false;
        }

        private void DlgProgress_Load(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(ThreadMainMethod);
        }

        private void ThreadMainMethod(object state)
        {
            try
            {
                m_ProgressActionDelegate(this);
                Invoke((Action) Close);
            }
            catch (Exception ex)
            {
                m_LastException = ex;
                Invoke((Action)Close);
            }
        }

        public static void Execute(string title, ProgressActionDelegate progressActionDelegate)
        {
            using (var dlgProgress = new DlgProgress(title, progressActionDelegate))
            {
                dlgProgress.ShowDialog();
                if (dlgProgress.LastException != null)
                {
                    throw dlgProgress.LastException;
                }
            }
        }

        #region IProgress Members

        #endregion
    }

    public interface IProgress
    {
        int ProgressValue { get; set; }
        string Message { set; }
        bool Cancel { get; }
        bool Undefined { get; set; }
    }
}
