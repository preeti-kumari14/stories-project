﻿using StoriesApp.Models;

namespace StoryApplication.Services
{
    public interface INewsService
    {
        Task<List<StoryDetailsDto>> GetStoryDetails();
    }
}
