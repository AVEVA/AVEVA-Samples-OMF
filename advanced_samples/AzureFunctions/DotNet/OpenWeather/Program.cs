using System;
using System.IO;
using System.Linq;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OSIsoft.Data.Http;
using OSIsoft.Identity;
using OSIsoft.Omf;
using OSIsoft.Omf.DefinitionAttributes;

namespace OpenWeatherFunction
{
    public static class Program
    {
        private static readonly string _openWeatherApiKey = Environment.GetEnvironmentVariable("OPEN_WEATHER_API_KEY");


        [FunctionName("CurrentWeather")]
        public static void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
        }

        /// <summary>
        /// Runs a generic HTTP GET request against the GitHub API
        /// </summary>
        private static string HttpGet(string url)
        {
            var uri = new Uri(url);
            var request = (HttpWebRequest)WebRequest.Create(uri);
            request.Accept = "application/json";
            request.UserAgent = "OSIGitHubStats";
            request.Headers.Add("Authorization", $"Bearer {GITHUB_PAT}");
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Sends a message to the OCS OMF endpoint
        /// </summary>
        private static object SendOmfMessage(OmfMessage omfMessage)
        {
            var serializedOmfMessage = OmfMessageSerializer.Serialize(omfMessage);
            return deviceOmfIngressService.SendOmfMessageAsync(serializedOmfMessage).Result;
        }
    }

    [OmfType(ClassificationType = ClassificationType.Dynamic, Name = "GitHubRepoStats", Description = "Represents daily statistics from a GitHub repository.")]
    public class GitHubRepoStats
    {
        [OmfProperty(IsIndex = true)]
        public DateTime Timestamp { get; set; }
        public bool Private { get; set; }
        public int Stargazers { get; set; }
        public int Watchers { get; set; }
        public int Forks { get; set; }
        public int OpenIssues { get; set; }
        public int Network { get; set; }
        public int Subscribers { get; set; }
        public int Views { get; set; }
        public int UniqueViews { get; set; }
        public int Clones { get; set; }
        public int UniqueClones { get; set; }
        public int Views2Week { get; set; }
        public int UniqueViews2Week { get; set; }
        public int Clones2Week { get; set; }
        public int UniqueClones2Week { get; set; }

        public GitHubRepoStats(JObject repo, JObject views, JObject clones)
        {
            // Log previous day's data since that day is complete
            Timestamp = DateTime.Today.AddDays(-1);
            Private = (bool)repo["private"];
            Stargazers = (int)repo["stargazers_count"];
            Watchers = (int)repo["watchers_count"];
            Forks = (int)repo["forks_count"];
            OpenIssues = (int)repo["open_issues_count"];
            Network = (int)repo["network_count"];
            Subscribers = (int)repo["subscribers_count"];

            // Find last day's views, if any
            var dailyViews = (JArray)views["views"];
            var dayViews = dailyViews.FirstOrDefault(d => (DateTime)d["timestamp"] == Timestamp);
            if (dayViews != null)
            {
                Views = (int)dayViews["count"];
                UniqueViews = (int)dayViews["uniques"];
            }

            // Get two week running view totals
            Views2Week = (int)views["count"];
            UniqueViews2Week = (int)views["uniques"];

            // Find last day's clones, if any
            var dailyClones = (JArray)clones["clones"];
            var dayClones = dailyClones.FirstOrDefault(d => (DateTime)d["timestamp"] == Timestamp);
            if (dayClones != null)
            {
                Clones = (int)dayClones["count"];
                UniqueClones = (int)dayClones["uniques"];
            }

            // Get two week running clone totals
            Clones2Week = (int)clones["count"];
            UniqueClones2Week = (int)clones["uniques"];
        }
    }
}
