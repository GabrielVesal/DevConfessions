using DevConfessions.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DevConfessions.Services
{
    public class JsonConfessionService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _jsonPath;

        public JsonConfessionService(IWebHostEnvironment env)
        {
            _env = env;
            _jsonPath = Path.Combine(_env.WebRootPath, "data", "confessions.json");
            InitializeFile();
        }

        private void InitializeFile()
        {
            if (!File.Exists(_jsonPath))
                File.WriteAllText(_jsonPath, "[]");
        }


        public void SaveConfessions(List<Confession> confessions) =>
            File.WriteAllText(_jsonPath, JsonSerializer.Serialize(confessions));

        public void AddConfession(Confession confession)
        {
            var confessions = GetAllConfessions();
            confession.Hashtags = ExtractHashtags(confession.Content);
            confession.Content = RemoveHashtags(confession.Content);
            confessions.Add(confession);
            SaveConfessions(confessions);
        }

        public List<string> ExtractHashtags(string content) =>
            Regex.Matches(content, @"#\w+").Select(m => m.Value).ToList();

        public string RemoveHashtags(string content) =>
            Regex.Replace(content, @"#\w+", "").Trim();

        public Confession? GetConfessionById(string id)
        {
            var confessions = GetAllConfessions();
            return confessions.FirstOrDefault(c => c.Id == id);
        }

        //public bool IsAuthor(string confessionId, string userToken)
        //{
        //    var confession = GetConfessionById(confessionId);
        //    return confession?.AuthorToken == userToken;
        //}

        public List<Confession> GetAllConfessions()
        {
            var confessions = JsonSerializer.Deserialize<List<Confession>>(File.ReadAllText(_jsonPath)) ?? new List<Confession>();
            return confessions.OrderByDescending(c => c.Votes).ToList();
        }
    }
}
