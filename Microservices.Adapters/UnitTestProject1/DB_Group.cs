using Microservices.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace UserCenter.DataModel
{
    public class DB_Group : IEntity
    {
        [Key]
        public int ID { get; set; }
        public string UUID { get; set; }
        public string Name { get; set; }
        public int GroupType { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public int DataStatus { get; set; }
    }
}
