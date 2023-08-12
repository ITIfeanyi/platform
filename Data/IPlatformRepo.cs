using redisAPI.Models;

namespace redisAPI.Data;

public interface IPlatformRepo
{
    void CreatePlatform(Platform platform);
    Platform? GetPlatformById(string id);
    IEnumerable<Platform?>? GetAllPlatform();
}