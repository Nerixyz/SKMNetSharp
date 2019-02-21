using System;
using System.Runtime.InteropServices;

namespace CoreClipboard.OSX
{
    internal class Clipboard : IClipboard
    {
        public void SetText(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var NSString = objc_getClass("NSString");
            var str = objc_msgSend(objc_msgSend(NSString, sel_registerName("alloc")), sel_registerName("initWithUTF8String:"), text);
            var dataType = objc_msgSend(objc_msgSend(NSString, sel_registerName("alloc")), sel_registerName("initWithUTF8String:"), NSPasteboardTypeString);

            var NSPasteboard = objc_getClass("NSPasteboard");
            var generalPasteboard = objc_msgSend(NSPasteboard, sel_registerName("generalPasteboard"));

            objc_msgSend(generalPasteboard, sel_registerName("clearContents"));
            objc_msgSend(generalPasteboard, sel_registerName("setString:forType:"), str, dataType);

            objc_msgSend(str, sel_registerName("release"));
            objc_msgSend(dataType, sel_registerName("release"));
        }

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
#pragma warning disable IDE1006 // Naming Styles
        public static extern IntPtr objc_getClass(string className);
#pragma warning restore IDE1006 // Naming Styles

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
#pragma warning disable IDE1006 // Naming Styles
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);
#pragma warning restore IDE1006 // Naming Styles

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
#pragma warning disable IDE1006 // Naming Styles
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, string arg1);
#pragma warning restore IDE1006 // Naming Styles

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
#pragma warning disable IDE1006 // Naming Styles
        public static extern IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg1, IntPtr arg2);
#pragma warning restore IDE1006 // Naming Styles

        [DllImport("/System/Library/Frameworks/AppKit.framework/AppKit")]
#pragma warning disable IDE1006 // Naming Styles
        public static extern IntPtr sel_registerName(string selectorName);
#pragma warning restore IDE1006 // Naming Styles

        const string NSPasteboardTypeString = "public.utf8-plain-text";
    }
}
