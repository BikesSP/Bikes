using Amazon.Runtime;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ClientService.Application.Services.Storage.Model;

namespace ClientService.Application.Services.Storage
{
    public class StorageService : IStorageService
    {
        private readonly IConfiguration _config;

        public StorageService(IConfiguration config)
        {
            _config = config;
        }
        public async Task<S3ResponseDto> UploadFileAsync(
                string name,
                MemoryStream inputStream,
                string bucketName)
        {
            string accessKey = _config["AwsConfiguration:AWSAccessKey"];
            string secretKey = _config["AwsConfiguration:AWSSecretKey"];

            Console.WriteLine($"Key: {accessKey}, Secret: {secretKey}");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };

            var response = new S3ResponseDto();
            try
            {
                var uploadRequest = new TransferUtilityUploadRequest()
                {
                    InputStream = inputStream,
                    Key = name,
                    BucketName = bucketName,
                    CannedACL = S3CannedACL.PublicRead
                };

                // initialise client
                using var client = new AmazonS3Client(credentials, config);

                // initialise the transfer/upload tools
                var transferUtility = new TransferUtility(client);

                // initiate the file upload
                await transferUtility.UploadAsync(uploadRequest);

                response.StatusCode = 201;
                response.Message = $"{name} has been uploaded sucessfully";

                GetPreSignedUrlRequest preSignedUrlRequest = new GetPreSignedUrlRequest
                {
                    BucketName = bucketName,
                    Key = name,
                    Expires = DateTime.Now.AddDays(7)
                };
                response.PreSignedUrl = client.GetPreSignedURL(preSignedUrlRequest);
            }
            catch (AmazonS3Exception s3Ex)
            {
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
            }

            return response;
        }

        public async Task<S3ResponseDto> DownloadFileAsync(
            string name,
            MemoryStream inputStream,
            string bucketName)
        {
            string accessKey = _config["AwsConfiguration:AWSAccessKey"];
            string secretKey = _config["AwsConfiguration:AWSSecretKey"];

            Console.WriteLine($"Key: {accessKey}, Secret: {secretKey}");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };

            var response = new S3ResponseDto();
            try
            {
                var downloadRequest = new TransferUtilityDownloadRequest()
                {
                    FilePath = "/Downloads",
                    Key = name,
                    BucketName = bucketName
                };

                // initialise client
                using var client = new AmazonS3Client(credentials, config);

                // initialise the transfer/upload tools
                var transferUtility = new TransferUtility(client);

                // initiate the file upload
                await transferUtility.DownloadAsync(downloadRequest);

                response.StatusCode = 201;
                response.Message = $"{name} has been downloaded sucessfully";
                Console.WriteLine($"{name} has been downloaded sucessfully");
            }
            catch (AmazonS3Exception s3Ex)
            {
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
                Console.WriteLine(s3Ex.Message);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            return response;
        }

        public async Task<IFormFile> GetContentFile(
           string name,
           string bucketName)
        {
            string accessKey = _config["AwsConfiguration:AWSAccessKey"];
            string secretKey = _config["AwsConfiguration:AWSSecretKey"];

            Console.WriteLine($"Key: {accessKey}, Secret: {secretKey}");

            var credentials = new BasicAWSCredentials(accessKey, secretKey);

            var config = new AmazonS3Config()
            {
                RegionEndpoint = Amazon.RegionEndpoint.APSoutheast1
            };

            var response = new S3ResponseDto();
            try
            {

                // initialise client
                using var client = new AmazonS3Client(credentials, config);

                Console.WriteLine(bucketName);

                var objectResponse = await client.GetObjectAsync(new GetObjectRequest
                {
                    BucketName = bucketName,
                    Key = name
                });

                var bytes = new byte[objectResponse.ResponseStream.Length];
                objectResponse.ResponseStream.Read(bytes, 0, bytes.Count());

                Console.WriteLine($"{name} has been loaded sucessfully");
                return new FormFile(objectResponse.ResponseStream, 0, objectResponse.ResponseStream.Length, name, name);
            }
            catch (AmazonS3Exception s3Ex)
            {
                response.StatusCode = (int)s3Ex.StatusCode;
                response.Message = s3Ex.Message;
                Console.WriteLine(s3Ex.Message);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.Message = ex.Message;
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public string getFullPathFile(string filePath)
        {
            string endpoint = _config["AwsConfiguration:AWSEndPoint"];
            return string.Concat(endpoint, filePath);
        }
    }

}
