using System.Text.Json;

using OrangeFinance.Application.Common.Interfaces;
using OrangeFinance.Infrastructure.Persistence;

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
            if (value is null)
                return;

            var valueSerialized = JsonSerializer.Serialize(value);

            await _context.Database.SetAddAsync(key, valueSerialized);
        }

        public async Task<T?> getCache<T>(string key)
        {
            if (await _context.Database.KeyExistsAsync(key))
            {
                var value = await _context.Database.SetMembersAsync(key);
                var valueDeserialized = JsonSerializer.Deserialize<T>(value.FirstOrDefault());
                return valueDeserialized;
            }
            return default;
        }
    }
}
