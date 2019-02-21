using System;
using System.Runtime.InteropServices;

namespace CoreClipboard.NotSupported
{
    internal class Clipboard : IClipboard
    {
        public void SetText(string text)
        {
            throw new NotSupportedException();
        }
    }
}
