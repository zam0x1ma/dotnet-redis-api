using System.Text.Json;
using RedisAPI.Models;
using StackExchange.Redis;

namespace RedisAPI.Data;

public class RedisPlatformRepository : IPlatformRepository
{
    private readonly IConnectionMultiplexer _redis;

    public RedisPlatformRepository(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }

    public void CreatePlatform(Platform platform)
    {
        if (platform is null)
        {
            throw new ArgumentNullException(nameof(platform));
        }

        var db = _redis.GetDatabase();

        var serializedPlatform = JsonSerializer.Serialize(platform);

        db.HashSet("HashPlatform", new HashEntry[]
            { new HashEntry(platform.Id, serializedPlatform) });
    }

    public IEnumerable<Platform?>? GetAllPlatforms()
    {
        var db = _redis.GetDatabase();

        var completeHash = db.HashGetAll("HashPlatform");

        if (completeHash.Length > 0)
        {
            var platforms = Array.ConvertAll(completeHash,
                    x => JsonSerializer.Deserialize<Platform>(x.Value))
                .ToList();

            return platforms;
        }

        return null;
    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();

        var platform = db.HashGet("HashPlatform", id);

        if (!string.IsNullOrEmpty(platform))
        {
            return JsonSerializer.Deserialize<Platform>(platform);
        }

        return null;
    }
}
