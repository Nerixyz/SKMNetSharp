using System;
using System.Runtime.InteropServices;

namespace CoreClipboard
{
    public interface IClipboard
    {
        void SetText(string text);
    }
}
