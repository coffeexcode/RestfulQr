using Amazon.Extensions.NETCore.Setup;
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Configuration;
using RestfulQr.Domain;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RestfulQr.Persistence.S3
{
    public class S3ImagePersistor : IImagePersistor
    {
        private readonly IAmazonS3 client;

        private readonly AWSOptions awsOptions;

        private readonly string bucketName;
        public S3ImagePersistor(IConfiguration config)
        {
            awsOptions = config.GetAWSOptions();
            bucketName = config.GetSection("S3")["BucketName"];
            client = awsOptions.CreateServiceClient<IAmazonS3>();
        }

        public async Task<byte[]?> GetImageAsync(ApiKey apiKey, string filename)
        {
            var path = string.Join(@"/", bucketName, apiKey.LocationId);

            var request = new GetObjectRequest
            {
                BucketName = path,
                Key = filename
            };

            using var response = await client.GetObjectAsync(request);
            using var stream = response.ResponseStream;

            if (response.HttpStatusCode != System.Net.HttpStatusCode.OK)
            {
                return null;
            }

            using var mem = new MemoryStream((int)response.ContentLength);
            await response.ResponseStream.CopyToAsync(mem);

            return mem.ToArray();
        }

        public async Task<bool> UploadAsync(ApiKey apiKey, string filename, byte[] content)
        {
            var path = string.Join(@"/", bucketName, apiKey.LocationId.ToString());

            var transferUtility = new TransferUtility(client);
            var request = new TransferUtilityUploadRequest()
            {
                BucketName = path,
                Key = filename
            };


            using var stream = new MemoryStream();
            await stream.WriteAsync(content);

            request.InputStream = stream;

            await transferUtility.UploadAsync(request);

            return true;
        }

        public async Task<bool> DeleteAsync(ApiKey apiKey, string filename)
        {
            var file = string.Join(@"/", apiKey.LocationId.ToString(), filename);

            var request = new DeleteObjectRequest
            {
                BucketName = bucketName,
                Key = file
            };

            try
            {
                await client.DeleteObjectAsync(request);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(ApiKey apiKey)
        {
            var toDelete = await GetS3Objects(apiKey);

            while (toDelete.Count > 0)
            {
                var request = new DeleteObjectsRequest
                {
                    BucketName = bucketName,
                };

                toDelete.ForEach(key => request.AddKey(key));

                if (toDelete.Count < 1000)
                {
                    request.AddKey(apiKey.LocationId.ToString());
                }

                try
                {
                    await client.DeleteObjectsAsync(request);
                }
                catch (Exception e)
                {
                    return false;
                }

                toDelete = await GetS3Objects(apiKey);
            }

            return true;
        }

        private async Task<List<string>> GetS3Objects(ApiKey apiKey)
        {
            var request = new ListObjectsV2Request
            {
                BucketName = bucketName,
                Prefix = apiKey.LocationId.ToString()
            };

            try
            {
                var results = await client.ListObjectsV2Async(request);

                return results.S3Objects.Select(x => x.Key).ToList();
            }
            catch (Exception)
            {
                return new List<string>();
            }
        }
    }
}