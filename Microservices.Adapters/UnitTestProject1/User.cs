using Microservices.Adapters.IDatabase;
using System.ComponentModel.DataAnnotations;

namespace ClassLibrary1
{
    public class User : IEntity
    {
        [Key]
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
