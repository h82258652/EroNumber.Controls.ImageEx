using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EroNumber.Cache
{
    public class DefaultDiskCache : IDiskCache
    {
        public DefaultDiskCache()
        {
            CacheFolderPath = Path.Combine(Path.GetTempPath(), Constants.DefaultCacheFolderName);
        }

        public DefaultDiskCache(string cacheFolderPath)
        {
            if (cacheFolderPath == null)
            {
                throw new ArgumentNullException(nameof(cacheFolderPath));
            }

            CacheFolderPath = cacheFolderPath;
        }

        public string CacheFolderPath { get; }

        public Task<long> CalculateAllSizeAsync()
        {
            if (Directory.Exists(CacheFolderPath))
            {
                return Task.FromResult(
                    Directory.EnumerateFiles(CacheFolderPath, "*", SearchOption.AllDirectories)
                        .Select(temp => new FileInfo(temp).Length)
                        .Sum());
            }

            return Task.FromResult(0L);
        }

        public Task<long> CalculateSizeAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheFileName = GetCacheFilePath(key);
            return Task.FromResult(new FileInfo(cacheFileName).Length);
        }

        public Task DeleteAllAsync()
        {
            var cacheFolderPath = CacheFolderPath;
            return Task.Run(() =>
            {
                if (Directory.Exists(cacheFolderPath))
                {
                    Directory.Delete(cacheFolderPath, true);
                }
            });
        }

        public Task DeleteAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheFilePath = GetCacheFilePath(key);
            return Task.Run(() => File.Delete(cacheFilePath));
        }

        public async Task<byte[]> GetAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheFilePath = GetCacheFilePath(key);
            using (var fileStream = File.OpenRead(cacheFilePath))
            {
                var buffer = new byte[fileStream.Length];
                await fileStream.ReadAsync(buffer, 0, buffer.Length);
                return buffer;
            }
        }

        public Task<bool> IsExistAsync(string key)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheFilePath = GetCacheFilePath(key);
            return Task.FromResult(File.Exists(cacheFilePath));
        }

        public async Task SetAsync(string key, byte[] data)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            var cacheFilePath = GetCacheFilePath(key);
            Directory.CreateDirectory(CacheFolderPath);
            using (var fileStream = File.Create(cacheFilePath))
            {
                await fileStream.WriteAsync(data, 0, data.Length);
            }
        }

        private string GetCacheFilePath(string key)
        {
            var extension = Path.GetExtension(key);
            using (var md5 = MD5.Create())
            {
                var buffer = Encoding.UTF8.GetBytes(key);
                var hashResult = md5.ComputeHash(buffer);
                var cacheFileName = BitConverter.ToString(hashResult).Replace("-", string.Empty) + extension;
                return Path.Combine(CacheFolderPath, cacheFileName);
            }
        }
    }
}