using System;
using System.Security.Principal;
using DataMigrations.Interfaces;

namespace DataMigrations.EntityFramework
{
    public static class MigrationExtensions
    {
        public static void Execute(this ISql sql, Action<string> sqlAction)
        {
            sqlAction(sql.ToString());
        }

        public static void Execute(this ISql sql, Action<string, bool, object> efSqlMigration)
        {
            efSqlMigration(sql.ToString(), false, null);
        }
    }
}