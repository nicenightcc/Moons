using Microservices.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace UserCenter.DataModel
{
    public class DB_UserGroup : IEntity
    {
        [Key]
        public int ID { get; set; }
        public int UserID { get; set; }
        public int GroupID { get; set; }
        public int CreateUserID { get; set; }
        public DateTime CreateTime { get; set; }
        public int DataStatus { get; set; }
        public virtual DB_User User { get; set; }
    }
}
