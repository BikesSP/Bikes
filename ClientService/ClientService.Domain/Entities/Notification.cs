using ClientService.Domain.Common.Enums.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class Notification : BaseAuditableEntity
    {
        public long Id { get; set; }
        public NotificationType Type {get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public DateTimeOffset Time { get; set; }
        public Guid AccountId { get; set; }
        public bool IsRead { get; set; }
        public DateTimeOffset ReadAt { get; set; }
    }
}
