using ServiceStack;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class ExponentPushToken: BaseAuditableEntity
    {
        [Key]
        public long Id { get; set; }
        public string Token { get; set; }
        public Guid AccountId { get; set; }
        public Account Account { get; set; }

    }
}
