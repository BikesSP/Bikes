using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Domain.Entities
{
    public class BasePaginationEntity<TEntity> where TEntity : class
    {
        public long Total { get; set; }
        public List<TEntity>? Data { get; set; }
    }
}
