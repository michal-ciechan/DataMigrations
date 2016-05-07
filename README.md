# DataMigrations

Code First Data Migrations - Allowing you to generate SQL to insert, update, and delete data which can be used as part of you're Code First Migrations

# Generating SQL

Data migrations allows you to generate SQL using a fluent syntax.

## Inserts

```csharp
// Fluent Interface
var sql = Insert

    // Set Table Name
    .Into("Person.Address")

    // Set Columns by using a string key any any T object
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
