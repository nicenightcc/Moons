using Microservices.Base;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace UserCenter.DataModel
{
    public class DB_User : IEntity
    {
        [Key]
        public int ID { get; set; }
        public string UUID { get; set; }
        public string UserName { get; set; }
        public string UserCode { get; set; }
        public string Password { get; set; }
        public int DataStatus { get; set; }
        public virtual ICollection<DB_User> Groups { get; set; }
    }
}
