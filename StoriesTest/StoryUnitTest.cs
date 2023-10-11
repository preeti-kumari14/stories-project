using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting.Logging;
using StoriesApp.Models;
using StoryApplication.Controllers;
using StoryApplication.Services;

namespace StoriesTest
{
    [TestClass]
    public class StoryUnitTest
    {
        private readonly ILogger<StoriesController> _logger;
        private readonly INewsService _newsService;
        private readonly IMemoryCache _cache;
        public StoryUnitTest(ILogger<StoriesController> logger, INewsService newsService, IMemoryCache cache)
        {
            _logger = logger;
            _newsService = newsService;
            _cache = cache;

        }

        /// <summary>
        /// Test Method for Unit Testing GetStoryDetails API method
        /// </summary>
        [TestMethod]
        public void TestMethod1()
        {
            var testStories = GetTestStories();
            var controller = new StoriesController(_logger, _newsService,_cache);

            var result = controller.GetStoryDetails();
            Assert.AreEqual(testStories, result?.Result?.Value?.Take(4));
        }

        /// <summary>
        /// Test Method to Test Caching
        /// </summary>
        [TestMethod]
        public string TestMethod2()
        {
            var controller = new StoriesController(_logger, _newsService, _cache);
            if(_cache != null)
            {
                return "Cache Available";
            }
            else { return "No Cache Available"; }

        }

        /// <summary>
        /// Method to create a mocking output of the Stories List
        /// </summary>
        /// <returns></returns>
        private IEnumerable<StoryDetailsDto> GetTestStories()
        {
            var testStories = new List<StoryDetailsDto>();
            testStories.Add(new StoryDetailsDto { StoryId = 37800533, StoryTitle = "What Plants Are Saying About Us", StoryUrl = "https://worldsensorium.com/what-plants-are-saying-about-us/" });
            testStories.Add(new StoryDetailsDto { StoryId = 37800528, StoryTitle = "The Usefulness of a Memory Guides Where the Brain Saves It", StoryUrl = "https://nautil.us/the-usefulness-of-a-memory-guides-where-the-brain-saves-it-410256/" });
            testStories.Add(new StoryDetailsDto { StoryId = 37800525, StoryTitle = "Finances of Mozilla [video]", StoryUrl = "https://www.youtube.com/watch?v=nIfj0qFtb1Y" });
            testStories.Add(new StoryDetailsDto { StoryId = 37800520, StoryTitle = "Al Gore Doesn't Say I Told You So", StoryUrl = "https://www.newyorker.com/news/q-and-a/al-gore-doesnt-say-i-told-you-so" });
            return testStories;
        }
    }
}