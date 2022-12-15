using Microsoft.AspNetCore.Mvc;
using RedisAPI.Data;
using RedisAPI.Models;

namespace RedisAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlatformsController : ControllerBase
{
    private readonly IPlatformRepository _repository;

    public PlatformsController(IPlatformRepository repository)
    {
        _repository = repository;
    }

    [HttpGet("{id}", Name = "GetPlatformById")]
    public ActionResult<Platform> GetPlatformById(string id)
    {
        var platform = _repository.GetPlatformById(id);

        if (platform is not null)
        {
            return Ok(platform);
        }

        return NotFound();
    }

    [HttpPost]
    public ActionResult<Platform> CreatePlatform(Platform platform)
    {
        _repository.CreatePlatform(platform);

        return CreatedAtRoute(nameof(GetPlatformById), new { Id = platform.Id }, platform);
    }

    [HttpGet]
    public ActionResult<IEnumerable<Platform>> GetAllPlatforms()
    {
        return Ok(_repository.GetAllPlatforms());
    }
}
