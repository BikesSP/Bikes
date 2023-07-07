using ClientService.Application.Services.Storage.Model;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientService.Application.Services.Storage
{
    public interface IStorageService
    {
        Task<S3ResponseDto> UploadFileAsync(string name, MemoryStream inputStream, string bucketName);
        Task<S3ResponseDto> DownloadFileAsync(string name, MemoryStream inputStream, string bucketName);
        Task<IFormFile> GetContentFile(
           string name,
           string bucketName);
        string getFullPathFile(string filePath);

    }
}
