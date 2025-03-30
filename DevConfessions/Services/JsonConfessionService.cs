using DevConfessions.Models;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace DevConfessions.Services
{
    public class JsonConfessionService
    {
        private readonly string _jsonPath;

        // Recebe o caminho DIRETO do JSON (configurado no Program.cs)
        public JsonConfessionService(string jsonPath)
        {
            _jsonPath = jsonPath;
            InitializeFile();
        }

        private void InitializeFile()
        {
            var directory = Path.GetDirectoryName(_jsonPath);
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);

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
