using ClientService.Domain.Common;
using ServiceStack.Redis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    [Table("Account")]
    public class Account : Vehicle
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }   
        public string? Phone { get; set; }
        public string? AvartarUlr { get; set; }
        public string? Card { get; set; }
        public Role Role { get; set; }
        public float averagePoint;
        public ObjectStatus AccountStatus { get; set; }
        public bool IsUpdated { get; set; }
        public string SubjectId { get; set; }
        public virtual List<Post> Application { get; set; }
        public ExponentPushToken ExponentPushToken { get; set; }
    }
}
