using System.Text.Json;
using redisAPI.Models;
using StackExchange.Redis;

namespace redisAPI.Data;

public class RedisPlatformRepo: IPlatformRepo
{
    private readonly IConnectionMultiplexer _redis;
    
    public RedisPlatformRepo(IConnectionMultiplexer redis)
    {
        _redis = redis;
    }
    public void CreatePlatform(Platform platform)
    {
        if (platform == null)
        {
            throw new ArgumentOutOfRangeException(nameof(platform));
        }

        var db = _redis.GetDatabase();
        var serialPlat = JsonSerializer.Serialize(platform);
       // db.StringSet(platform.Id, serialPlat);
        // db.SetAdd("platformsSet", serialPlat);
        db.HashSet("hashPlatform", new HashEntry[]{new HashEntry(platform.Id, serialPlat)});
    }

    public Platform? GetPlatformById(string id)
    {
        var db = _redis.GetDatabase();
       // var plat = db.StringGet(id);
       var plat = db.HashGet("hashPlatform",id);
        return !string.IsNullOrEmpty(plat) ? JsonSerializer.Deserialize<Platform>(plat) : null;
    }

    public IEnumerable<Platform?>? GetAllPlatform()
    {
        var db = _redis.GetDatabase();
        //var completeSet = db.SetMembers("platformsSet");
        var completeHash = db.HashGetAll("hashPlatform");
        if (completeHash.Length <= 0) return null;
        var obj = Array.ConvertAll(completeHash, val => JsonSerializer.Deserialize<Platform>(val.Value)).ToList();
        return obj;
    }
}