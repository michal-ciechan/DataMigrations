using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using DataMigrations.Helpers;
using DataMigrations.Interfaces;

namespace DataMigrations
{
    public class Insert : ISql
    {
        private bool _identityInsert;
        
        public string Table { get; set; }

        public List<KeyValuePair<string,string>> Setters { get; set; }

        public Insert()
        {
            Setters = new List<KeyValuePair<string, string>>();
        }

        public Insert(string table)
            : this()
        {
            Table = table;
        }

        public Insert IntoTable(string table)
        {
            Table = table;

            return this;
        }

        public Insert Set<T>(Expression<Func<T>> expression)
        {
            var column = expression.GetMemberName();

            var value = expression.Compile()().ToSql();

            Add(column, value);

            return this;
        }

        private void Add(string column, string value)
        {
            Setters.Add(new KeyValuePair<string, string>(column, value));
        }

        public Insert Set<T>(string column, T value)
        {
            Add(column, value.ToSql());

            return this;
        }

        public override string ToString()
        {
            if(Table == null) throw new ArgumentNullException(nameof(Table));

            var prefix = _identityInsert ? GetIdentityInsert(true) : null;
            var suffix = _identityInsert ? GetIdentityInsert(false) : null;

            return $"{prefix}INSERT INTO {Table} ({GetColumns()}) VALUES ({GetValues()}){suffix}";
        }

        private string GetValues()
        {
            return string.Join(", ", Setters.Select(x => x.Value));
        }

        private string GetColumns()
        {
            return string.Join(", ", Setters.Select(x => x.Key));
        }

        public static Insert Into(string table)
        {
            return new Insert(table);
        }

        public Insert WithIdentityInsert()
        {
            _identityInsert = true;

            return this;
        }

        private string GetIdentityInsert(bool val)
        {
            var swtch = val ? "ON" : "OFF";
            return $"\r\nSET IDENTITY_INSERT {Table} {swtch};\r\n";
        }
    }
}