using System.Diagnostics.CodeAnalysis;
using DataMigrations.Helpers;
using FluentAssertions;
using NUnit.Framework;

namespace DataMigrations.Tests.README
{
    [TestFixture]
    public class ConvertingCSharpValuesToSql
    {
        [SetUp]
        public void A_TestSetup()
        {
        }

        [TearDown]
        public void A_TestTeardown()
        {
            SqlConverter.ResetCustomConverters();
        }

        [Test]
        public void CustomConverters()
        {
            // Add custom converter for Boolean type
            SqlConverter.SetCustom<bool>(obj
                => obj ? "Custom True" : "Custom False");

            false.ToSql().Should().Be("Custom False");
            true.ToSql().Should().Be("Custom True");

            // Add custom converter for Nullable<Int32> type
            SqlConverter.SetCustom<int?>(obj
                => obj == null ? "'NO INT'" : $"'{obj.ToString()}'");

            ((int?) 1337).ToSql().Should().Be("'1337'");
            ((int?) null).ToSql().Should().Be("'NO INT'");
        }

        [Test]
        [SuppressMessage("ReSharper", "RedundantCast")]
        public void DefaultConverters()
        {
            // Booleans should be converted to 0/1, null bool? should return NULL
            ((bool) false).ToSql().Should().Be("0");
            ((bool?) true).ToSql().Should().Be("1");
            ((bool?) null).ToSql().Should().Be("NULL");

            // Ints should bring back raw value, null should return NULL
            ((int) 1337).ToSql().Should().Be("1337");
            ((int?) 1337).ToSql().Should().Be("1337");
            ((int?) null).ToSql().Should().Be("NULL");

            // Strings should be single quoted
            ((string) "Data").ToSql().Should().Be("'Data'");
            ((string) "").ToSql().Should().Be("''");
            ((string) null).ToSql().Should().Be("NULL");
        }
    }
}