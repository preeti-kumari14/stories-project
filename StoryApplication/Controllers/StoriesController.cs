using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using StoriesApp.Models;
using StoryApplication.Controllers;
using StoryApplication.Services;

namespace StoryApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoriesController : ControllerBase
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly INewsService _newsService;
        private readonly IMemoryCache _cache;
        private const string storiesCacheKey = "storiesDetails";
        public StoriesController(ILogger<StoriesController> logger, INewsService newsService , IMemoryCache cache)
        {
            _logger = logger;
            _newsService = newsService;
            _cache = cache;

        }
        [HttpGet(Name = "GetNewStories")]
        public async Task<ActionResult<IEnumerable<StoryDetailsDto>>> GetStoryDetails()
        {
            if (_cache.TryGetValue(storiesCacheKey, out IEnumerable<StoryDetailsDto> result))
            {
                _logger.Log(LogLevel.Information, "Employee list found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Story Details not found in cache. Fetching from api.");

                result = await _newsService.GetStoryDetails();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromSeconds(60))
                    .SetAbsoluteExpiration(TimeSpan.FromSeconds(3600))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
                _cache.Set(storiesCacheKey, result, cacheEntryOptions);
            }
            return Ok(result);
        }
    }
}
