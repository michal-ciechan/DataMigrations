using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace DataMigrations.Tests
{
    [TestFixture]
    public class SqlConverterTests
    {

        [SetUp]
        public void A_TestSetup()
        {
            SqlConverter.ResetCustomConverters();
        }

        [Test]
        [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
        public void ToSqlGeneric_NullableBool_ShouldRevertToNonGeneric()
        {
            SqlConverter.ToSql<bool?>(false).Should().Be("0");
        }
        [Test]
        [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
        public void ToSqlGeneric_Bool_ShouldRevertToNonGeneric()
        {
            SqlConverter.ToSql<bool>(true).Should().Be("1");
        }
        [Test]
        [SuppressMessage("ReSharper", "InvokeAsExtensionMethod")]
        public void ToSqlGeneric_String_ShouldRevertToNonGeneric()
        {
            SqlConverter.ToSql<string>("Test").Should().Be("'Test'");
        }
    }
}