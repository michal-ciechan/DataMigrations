using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using DataMigrations.Interfaces;

namespace DataMigrations.Extensions
{
    public static class ClipboardExtensions
    {
        [DllImport("user32.dll")]
        public static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        public static extern bool CloseClipboard();

        [DllImport("user32.dll")]
        public static extern bool SetClipboardData(uint uFormat, IntPtr data);

        [DllImport("user32.dll")]
        public static extern IntPtr GetClipboardData(uint uFormat);

        /// <summary>
        /// Copies data to clipboard. Potential Memory Leak so do not use in production!
        /// </summary>
        /// <param name="sql"></param>
        public static void ToClipboard(this ISql sql)
        {
            OpenClipboard(IntPtr.Zero);

            var text = sql.ToString();
            var ptr = Marshal.StringToHGlobalUni(text);

            SetClipboardData(13, ptr); // 13 = Unicode

            if (!CloseClipboard())
            {
                throw new Exception("Could not close Clipboard");
            }
        }
    }
}
