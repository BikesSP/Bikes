using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class BaseDeletableEntity : BaseAuditableEntity
    {
        public DateTimeOffset DeletedAt { get; set; }

        public string? DeletedBy { get; set; }
    }

}
