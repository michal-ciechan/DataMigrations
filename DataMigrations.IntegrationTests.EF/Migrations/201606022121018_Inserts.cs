using DataMigrations.EntityFramework;

namespace DataMigrations.IntegrationTests.EF.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
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
}
