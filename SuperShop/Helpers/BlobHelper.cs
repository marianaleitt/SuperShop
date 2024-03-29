﻿using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;


namespace SuperShop.Helpers
{
    public class BlobHelper : IBlobHelper
    {
        private readonly CloudBlobClient _blobClient;

        public BlobHelper(IConfiguration configuration)
        {
                //_configuration = configuration;
            string keys = configuration["Blob:ConnectionString"];
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(keys);
            _blobClient =storageAccount.CreateCloudBlobClient();
        }
        public async Task<Guid> UploadBlobAsync(IFormFile file, string containterName)
        {
            Stream stream = file.OpenReadStream();
            return await UploadStreamAsync(stream, containterName);
        }

        public async Task<Guid> UploadBlobAsync(byte[] file, string containterName)
        {
            MemoryStream stream = new MemoryStream(file);
            return await UploadStreamAsync(stream, containterName);
        }

        public async Task<Guid> UploadBlobAsync(string image, string containterName)
        {
            Stream stream = File.OpenRead(image);
            return await UploadStreamAsync(stream, containterName);
        }

        public Task<Guid> UploadImageAsync(IFormFile imageFile, string v)
        {
            throw new NotImplementedException();
        }

        private async Task<Guid> UploadStreamAsync(Stream stream, string containterName)
        {
            Guid name = Guid.NewGuid();
            CloudBlobContainer container = _blobClient.GetContainerReference(containterName);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference($"{name}");
            await blockBlob.UploadFromStreamAsync(stream);
            return name;
        }
    }
}
