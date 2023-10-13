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

        /// <summary>
        /// HTTP Get Controller Method  to Fetch Stories
        /// if cache is present list will be pulled from cache else from api
        /// </summary>
        /// <returns></returns>
        [HttpGet(Name = "GetNewStories")]

        public async Task<ActionResult<StoriesDto>> GetStoryDetails()
        {
            StoriesDto storiesDto = new StoriesDto();
            if (_cache.TryGetValue(storiesCacheKey, out IEnumerable<StoryDetailsDto> result))
            {
                storiesDto.FromCache = true;
                storiesDto.stories = result;
                _logger.Log(LogLevel.Information, "Story list found in cache.");
            }
            else
            {
                _logger.Log(LogLevel.Information, "Story Details not found in cache. Fetching from api.");

                result = await _newsService.GetStoryDetails();
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                    .SetAbsoluteExpiration(TimeSpan.FromMinutes(30))
                    .SetPriority(CacheItemPriority.Normal)
                    .SetSize(1024);
                _cache.Set(storiesCacheKey, result, cacheEntryOptions);

                storiesDto.FromCache = false;
                storiesDto.stories = result;
            }
            return Ok(storiesDto);
        }
    }
}
