using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SKMNET.Client
{
    public partial class LightingConsole
    {
        private void Connection_Errored(object sender, Exception e)
        {
            OnErrored(e);
        }

        public event EventHandler<Exception> Errored;
        protected virtual void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public event EventHandler Pieps;
        public virtual void OnPieps(object sender) { Pieps?.Invoke(sender, new EventArgs()); }

    }
}
