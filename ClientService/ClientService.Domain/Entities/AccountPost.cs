
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class AccountPost
    {
        [ForeignKey("Post")]
        public long ApplicationId { get; set; }
        [ForeignKey("Account")]
        public Guid ApplierId { get; set; }
    }
}
