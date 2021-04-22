using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace eRentSolution.Application.Common
{
    public class FileStorageService : IStorageService
    {
        private readonly string _userContentFolder;
        private const string USER_CONTENT_FOLDER_NAME = "user-content";

        public FileStorageService(IWebHostEnvironment webHostInviroment)
        {
            _userContentFolder = Path.Combine(webHostInviroment.WebRootPath, USER_CONTENT_FOLDER_NAME);
        }
        public string GetFileUrl(string fileName)
        {
            return $"/{USER_CONTENT_FOLDER_NAME}/{fileName}";
        }
        public async Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);
            if (Directory.Exists(_userContentFolder) == false)
            {
                Directory.CreateDirectory(_userContentFolder);
            }
            var output = new FileStream(filePath, FileMode.Create);
            await mediaBinaryStream.CopyToAsync(output);
        }


        public int DeleteFile(string fileName)
        {
            var filePath = Path.Combine(_userContentFolder, fileName);

            int numberOfRetries = 30;
            int delayOnRetry = 1000;
            for(int i=1; i<numberOfRetries; ++i)
            {
                try
                {
                    //if (File.Exists(filePath))
                    //{
                    //    await Task.Run(() => File.Delete(filePath));
                    //}
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                    return 1;
                }catch(Exception e) when (i<=numberOfRetries)
                {
                    Thread.Sleep(delayOnRetry);
                }
            }
            return -1;
        }
    }
}
