using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Infrastructure.Repositories.PostRepository
{
    public class PostRepository : BaseRepository<ApplicationDbContext, Post>, IPostRepository
    {
        public PostRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
