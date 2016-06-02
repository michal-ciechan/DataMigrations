using System.ComponentModel.DataAnnotations.Schema;

namespace DataMigrations.IntegrationTests.EF
{
    public class Person
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}