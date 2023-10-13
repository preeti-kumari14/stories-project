namespace StoriesApp.Models
{
    public class StoryDetailsDto
    {
        public int StoryId { get; set; }
        public string? StoryTitle { get; set;}
        public string? StoryUrl { get;set; }
        public bool FromCache { get; set; } = false;
    }

    public class StoriesDto
    {
        public IEnumerable<StoryDetailsDto> stories { get; set; }
        public bool FromCache { get; set; } = false;
    }
}
