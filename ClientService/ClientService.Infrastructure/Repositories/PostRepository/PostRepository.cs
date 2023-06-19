using ClientService.Domain.Entities;
using ClientService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Repository.Repositories.BaseRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Amazon.S3.Util.S3EventNotification;

namespace ClientService.Infrastructure.Repositories.PostRepository
{
    public class PostRepository : BaseRepository<ApplicationDbContext, Post>, IPostRepository
    {
        private readonly DbContext _dbContext;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            _dbContext = context ?? throw new ArgumentNullException(nameof(context));
        }

   
    }
}
