using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace eRentSolution.Application.Common
{
    public class FileStorageService : IStorageService
    {
        public Task DeleteFileAsync(string fileName)
        {
            throw new NotImplementedException();
        }

        public string GetFileUrl(string fileName)
        {
            throw new NotImplementedException();
        }

        public Task SaveFileAsync(Stream mediaBinaryStream, string fileName)
        {
            throw new NotImplementedException();
        }
    }
}
