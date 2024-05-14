using OrangeFinance.Application.Common.Interfaces;
using OrangeFinance.Infrastructure.Persistence;
using System.Text.Json;

namespace OrangeFinance.Infrastructure
{
    internal sealed class CacheRepository : ICacheRepository
    {
        private readonly RedisDBContext _context;
        public CacheRepository(RedisDBContext context)
        {
            _context = context;
        }

        public async Task setCache<T>(string key, T value)
        {
            var valueSerialized = JsonSerializer.Serialize(value);

            await _context.Database.SetAddAsync(key, valueSerialized);
        }

        public async Task<T?> getCache<T>(string key)
        {
            var exist = await _context.Database.KeyExistsAsync(key);

            if (!exist) return default(T?);

            var response = await _context.Database.StringGetAsync(key);

            return JsonSerializer.Deserialize<T>(response!) ?? default;
        }
    }
}
