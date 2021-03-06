# DataMigrations

Code First Data Migrations - Allowing you to generate SQL to insert, update, and delete data which can be used as part of you're Code First Migrations

# Table of Contents

<!-- TOC depthFrom:1 depthTo:6 withLinks:1 updateOnSave:1 orderedList:0 -->

- [DataMigrations](#datamigrations)
- [Table of Contents](#table-of-contents)
- [Generating SQL](#generating-sql)
	- [Inserts](#inserts)
	- [Updates](#updates)
	- [Deletes](#deletes)
- [Converting C# Values to SQL](#converting-c-values-to-sql)
	- [Default converters](#default-converters)
	- [Custom Converters](#custom-converters)

<!-- /TOC -->

# Generating SQL

Data migrations allows you to generate SQL using a fluent syntax.

## Inserts

```csharp
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
```

## Updates

TODO:

## Deletes

```csharp
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
```

# Converting C# Values to SQL

DataMigrations provides a handfull of `ToSql()` extension methods which return back the SQL value as text such as the following:

## Default converters

```csharp
// Booleans should be converted to 0/1, null bool? should return NULL
((bool)  false).ToSql().Should().Be("0");
((bool?) true ).ToSql().Should().Be("1");
((bool?) null ).ToSql().Should().Be("NULL");

// Ints should bring back raw value, null should return NULL
((int)  1337).ToSql().Should().Be("1337");
((int?) 1337).ToSql().Should().Be("1337");
((int?) null).ToSql().Should().Be("NULL");

// Strings should be single quoted
((string) "Data").ToSql().Should().Be("'Data'");
((string) ""    ).ToSql().Should().Be("''");
((string) null  ).ToSql().Should().Be("NULL");
```

## Custom Converters

```csharp
// Add custom converter for Boolean type
SqlConverter.CustomConversions.Add(typeof(bool),
    obj => (bool) obj ? "Custom True" : "Custom False");

false.ToSql().Should().Be("Custom False");
true .ToSql().Should().Be("Custom True");

// Add custom converter for Nullable<Int32> type
SqlConverter.CustomConversions.Add(typeof(int?),
    obj => (int?)obj == null ? "'NO INT'" : $"'{obj.ToString()}'");

((int?)1337).ToSql().Should().Be("'1337'");
((int?)null).ToSql().Should().Be("'NO INT'");
```

# Additional Features

## Identity Insert

Insert Auto-Identity values by using the `Insert.WithIdentityInsert()` method
which will add `SET IDENTITY_INSERT Table ON|OFF` statements
to the start and end of the sql query.


```csharp

var sql = Insert
		.Into("Person")
		.WithIdentityInsert() // <-- Set Identity Insert On
		.Set("Id", 2) // Identity Column
		.ToString();

sql.Should().Be(
		"\r\nSET IDENTITY_INSERT Person ON;\r\n" +

		"INSERT INTO Person (Id) VALUES (2)" +

		"\r\nSET IDENTITY_INSERT Person OFF;\r\n");
```

## EntityFramework Extensions

There is an extension method callsed `Execute` for `ISql` (base class for `Insert`,`Delete`, etc helper classes) which takes `Action<string, bool, object>` which is the signature for the `DbMigration.Sql(..)` method which allows you to do the following.

```csharp
using DataMigrations.EntityFramework;

public partial class Inserts : DbMigration
{
		public override void Up()
		{
				Insert.Into("Person")
						.Set("Name", "Michal Ciechan")
						.Execute(Sql); // <-- Extension method which executes SQL
		}

		public override void Down()
		{
				Delete.From("Person")
						.Where("Name", "Michal Ciechan")
						.Execute(Sql); // <-- Extension method which executes SQL
		}
}
```

## Clipboard Extensions

There is also a `ISql.ToClipboard()` extension which copies the SQL to the clipboard, for people who do not use EF CodeFirst Migrations, and want to generate the SQL easily and paste into a file which they can add to their SQL release process.

**WARNING** - This could potentially leak memory, so do not use in production. Only in test/dev environment to easily generate SQL and copy it to the clipboard.

```csharp
Insert.Into("Person")
		.Set("Name", "Michal Ciechan")
		.ToClipboard();

var str = GetClipboardData();

str.Should().Be("INSERT INTO Person (Name) VALUES ('Michal Ciechan')");
```
