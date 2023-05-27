using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class BaseAuditableEntity
    {
        public DateTimeOffset CreatedAt { get; set; }

        public string? CreatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public string? UpdatedBy { get; set; }
    }

}
