using ServiceStack.DataAnnotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Common
{
    public enum PostStatus
    {
        [Description("CREATED")]
        Created,
        [Description("COMPLETED")]
        Completed
    }
}
