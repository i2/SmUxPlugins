using System.Drawing;
using System.Windows.Forms;

namespace PluginSystem.Plugin
{
    public abstract class SuperMemoExtension
    {
        public virtual int Order
        {
            get
            {
                return 0;
            }
        }

        public virtual bool Active
        {
            get
            {
                return true;
            }
        }

        public abstract string Name
        { 
            get;
        }

        public virtual Image Image
        { 
            get
            {
                return null;
            }
        }

        public virtual Keys GetShortcut()
        {
            return Keys.None;
        }

        public abstract void Execute();

        public virtual bool Enabled
        {
            get
            {
                return true; 
            }
        }
    }
}