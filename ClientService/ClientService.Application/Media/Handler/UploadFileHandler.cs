using ClientService.Application.Media.Command;
using ClientService.Application.Media.Model;
using ClientService.Application.Services.Storage;
using ClientService.Domain.Wrappers;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Media.Handler
{
    public class UploadFileHandler : IRequestHandler<UploadFileRequest, Response<UploadFileResponse>>
    {
        private readonly IStorageService _storageService;

        public UploadFileHandler(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<Response<UploadFileResponse>> Handle(UploadFileRequest request, CancellationToken cancellationToken)
        {
            await using var memoryStream = new MemoryStream();
            await request.File.CopyToAsync(memoryStream);

            var result = await _storageService.UploadFileAsync(request.File.FileName, memoryStream, "prn221");

            UploadFileResponse response = new UploadFileResponse();
            response.Path = _storageService.getFullPathFile("/" + request.File.FileName);
            return new Response<UploadFileResponse>()
            {
                Code = 0,
                Data = response
            };
        }
    }
}
