using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using DataMigrations.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace DataMigrations.Tests.Extensions
{
    [TestFixture]
    public class ClipboardExtensionsTests
    {

        [Test]
        public void ToClipboard()
        {
            Insert.Into("Person")
                .Set("Name", "Michal Ciechan")
                .ToClipboard(); // <-- Inserts into clipboard

            var str = GetClipboardData();

            str.Should().Be("INSERT INTO Person (Name) VALUES ('Michal Ciechan')");
        }

        private static string GetClipboardData()
        {
            ClipboardExtensions.OpenClipboard(IntPtr.Zero);

            var ptr = ClipboardExtensions.GetClipboardData(13); // 13 = Unicode

            var str = Marshal.PtrToStringUni(ptr);

            ClipboardExtensions.CloseClipboard();

            Marshal.FreeHGlobal(ptr);
            return str;
        }
    }
}