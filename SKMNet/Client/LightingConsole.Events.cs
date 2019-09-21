using System;
using SKMNET.Client.Events;

namespace SKMNET.Client
{
    public partial class LightingConsole
    {
        private void Connection_Errored(object sender, Exception e) => OnErrored(e);

        public event EventHandler<Exception> Errored;
        private void OnErrored(Exception data) { Errored?.Invoke(this, data); }

        public event EventHandler Pieps;
        public void OnPieps(object sender) { Pieps?.Invoke(sender, EventArgs.Empty); }


        public event EventHandler<BLampEventArgs> LampUpdate;
        public event EventHandler<SkIntensityChangedEventArgs> SkIntensityChanged;
        public event EventHandler<SkAttrChangedEventArgs> SkAttrChanged;

    }
}
