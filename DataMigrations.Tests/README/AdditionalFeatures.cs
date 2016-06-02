using FluentAssertions;
using NUnit.Framework;

namespace DataMigrations.Tests.README
{
    [TestFixture]
    public class AdditionalFeaturesTests
    {
        [Test]
        public void IdentityInsert()
        {
            /*
            Insert Auto-Identity values by using the `Insert.WithIdentityInsert()` method
            which will add `SET IDENTITY_INSERT Table ON|OFF` statements 
            to the start and end of the sql query.
            */
            var sql = Insert
                .Into("Person")
                .WithIdentityInsert() // <-- Set Identity Insert On
                .Set("Id", 2) // Identity Column
                .ToString();

            sql.Should().Be(
                "\r\nSET IDENTITY_INSERT Person ON;\r\n" + 

                "INSERT INTO Person (Id) VALUES (2)" +

                "\r\nSET IDENTITY_INSERT Person OFF;\r\n");
        }
    }
}