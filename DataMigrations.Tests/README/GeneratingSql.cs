using System;
using System.Diagnostics.CodeAnalysis;
using FluentAssertions;
using NUnit.Framework;

namespace DataMigrations.Tests.README
{
    [TestFixture]
    public class GeneratingSql
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
        public void Inserts()
        {
            var AddressLine1 = "A Street";

            // Fluent Interface
            var sql = Insert

                // Set Table Name
                .Into("Person.Address")

                // Set Columns by using a string key and any T object
                .Set("City","London")

                // Set Columns by using a member expression 
                // e.g. var AddressLine1 = "A Street";
                // Member name is used as the column name
                // Member value is used as the value
                .Set(() => AddressLine1)
                
                // Call ToString() to generate SQL
                .ToString();

            sql.Should().Be(
                "INSERT INTO Person.Address (City, AddressLine1) " +
                "VALUES ('London', 'A Street')");
        }

        [Test]
        public void Deletes()
        {
            // ReSharper disable once InconsistentNaming
            string AddressLine1 = null;

            // Fluent Interface
            var sql = Delete

                // Set Table Name
                .From("Person.Address")

                // Filter Columns by using a string key and any T object
                .Where("City", "London")

                // Filter Columns by using a member expression 
                // e.g. string AddressLine1 = null;
                // Member name is used as the column name
                // Member value is used as the value
                .Where(() => AddressLine1)

                // Call ToString() to generate SQL
                .ToString();

            sql.Should().Be(
                "DELETE FROM Person.Address " +
                "WHERE City = 'London' AND AddressLine1 IS NULL");
        }
    }
}