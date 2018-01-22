using System.Threading.Tasks;

namespace EroNumber.Cache
{
    public interface IDiskCache
    {
        string CacheFolderPath { get; }

        Task<long> CalculateAllSizeAsync();

        Task<long> CalculateSizeAsync(string key);

        Task DeleteAllAsync();

        Task DeleteAsync(string key);

        Task<byte[]> GetAsync(string key);

        Task<bool> IsExistAsync(string key);

        Task SetAsync(string key, byte[] data);
    }
}