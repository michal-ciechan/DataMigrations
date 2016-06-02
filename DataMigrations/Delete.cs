using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using DataMigrations.Helpers;
using DataMigrations.Interfaces;

namespace DataMigrations
{
    public class Delete : ISql
    {
        public string Table { get; set; }

        public List<string> WhereClauses { get; set; }

        public Delete()
        {
            WhereClauses = new List<string>();
        }

        public Delete(string table)
            : this()
        {
            Table = table;
        }

        public Delete FromTable(string table)
        {
            Table = table;

            return this;
        }

        public Delete Where<T>(string column, T value)
        {
            var clause = CreateWhereClause(column, value.ToSql());

            Add(clause);

            return this;
        }

        public Delete Where<T>(Expression<Func<T>> expression)
        {
            var column = expression.GetMemberName();

            var value = expression.Compile()().ToSql();

            var clause = CreateWhereClause(column, value);

            Add(clause);

            return this;
        }

        private string CreateWhereClause(string column, string value)
        {
            var comparison = value == "NULL" ? "IS NULL" : $"= {value}";

            return $"{column} {comparison}";
        }

        public void Add(string whereClause)
        {
            WhereClauses.Add(whereClause);
        }

        public override string ToString()
        {
            if(Table == null) throw new ArgumentNullException(nameof(Table));
            
            return $"DELETE FROM {Table} WHERE {GetWhere()}";
        }

        private string GetWhere()
        {
            return string.Join(" AND ", WhereClauses.Select(x => x));
        }

        public static Delete From(string table)
        {
            return new Delete(table);
        }
    }
}