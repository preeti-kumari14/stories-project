using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StoriesApp.Models;
using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Nodes;
using static System.Net.WebRequestMethods;

namespace StoryApplication.Services
{
    public class NewsService : INewsService
    {
        /// <summary>
        /// Method to fetch new stories ID list from API 
        /// post fetching details for stories ,filter out stories with blank URI
        /// </summary>
        /// <returns></returns>
        public async Task<List<StoryDetailsDto>> GetStoryDetails()
        {
            try
            {
                string reqURL = "https://hacker-news.firebaseio.com/v0/item/";
                var storyDetailsList = new List<StoryDetailsDto>();
                var newStoryList = await GetNewStories();

                //foreach (var story in newStoryList.Take(213))
                foreach (var story in newStoryList.Take(20))
                {
                    string url = reqURL + story.ToString() + ".json?print=pretty";
                    using (var httpClient = new HttpClient())
                    {
                        using (var response = await httpClient.GetAsync(url))
                        {

                            string apiResponse = await response.Content.ReadAsStringAsync();

                            JObject? storyJson = JsonConvert.DeserializeObject(apiResponse) as JObject;
                            storyDetailsList.Add(new StoryDetailsDto
                            {
                                StoryId = JsonValue<int>(storyJson, "id"),
                                StoryTitle =JsonValue<string>(storyJson, "title"),
                                StoryUrl= JsonValue<string>(storyJson, "url"),
                            });
                        }
                    }
                }
                var items = storyDetailsList.Where(x => x.StoryUrl == null).Select(i => i.StoryId);
                storyDetailsList.RemoveAll(x => items.Contains(x.StoryId));

                return storyDetailsList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// Get New Stories details from API based on Stories ID
        /// </summary>
        /// <returns></returns>
        private async Task<List<Int32>> GetNewStories()
        {
            var result = new List<Int32>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("https://hacker-news.firebaseio.com/v0/newstories.json?print=pretty"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    apiResponse = apiResponse.Replace("[", "").Replace("]", "");
                    result = apiResponse.Split(',').Select(int.Parse).ToList();
                }
            }
            return result;
        }
        /// <summary>
        /// Method to deserialize Json to get value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        private T? JsonValue<T>(JObject json, string propertyName)
        {
            var property = json.Property(propertyName);
            return property == null ? default : property.Value.ToObject<T>();

        }
    }
}
