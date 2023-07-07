using ClientService.Application.Media.Model;
using ClientService.Domain.Wrappers;
using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Media.Command
{
    public class UploadFileRequest : IRequest<Response<UploadFileResponse>>
    {
        public IFormFile File { get; set; }

    }
}
