using DevConfessions.Models;
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.RegularExpressions;

namespace DevConfessions.Services
{
    public class JsonConfessionService
    {
        private readonly FirebaseClient _firebase;
        private const string ConfessionsPath = "confessions";

        public JsonConfessionService(FirebaseClient firebase)
        {
            _firebase = firebase;
        }

        public async Task AddConfession(Confession confession)
        {
            confession.Hashtags = ExtractHashtags(confession.Content);
            confession.Content = RemoveHashtags(confession.Content);

            if (!string.IsNullOrEmpty(confession.Id))
            {
                await UpdateConfessionAsync(confession);
            }
            else
            {
                var result = await _firebase
                    .Child(ConfessionsPath)
                    .PostAsync(confession);
                confession.Id = result.Key;
            }
        }

        public async Task<int> IncrementVote(string id)
        {
            var confession = await GetConfessionById(id);
            if (confession != null)
            {
                confession.Votes++;
                await UpdateConfessionAsync(confession);
                return confession.Votes; // Retorna o novo valor de votos
            }
            throw new Exception("Confissão não encontrada"); // Ou retorne 0 se preferir
        }

        public async Task UpdateConfessionAsync(Confession confession)
        {
            await _firebase
                .Child(ConfessionsPath)
                .Child(confession.Id)
                .PutAsync(confession);
        }

        public List<string> ExtractHashtags(string content) =>
            Regex.Matches(content, @"#\w+").Select(m => m.Value).ToList();

        public string RemoveHashtags(string content) =>
            Regex.Replace(content, @"#\w+", "").Trim();

        public async Task<Confession?> GetConfessionById(string id)
        {
            var confession = await _firebase
                .Child(ConfessionsPath)
                .Child(id)
                .OnceSingleAsync<Confession>();

            if (confession != null)
            {
                confession.Id = id;
            }

            return confession;
        }

        public async Task<List<Confession>> GetAllConfessions()
        {
            var result = await _firebase
                .Child(ConfessionsPath)
                .OnceAsync<Confession>();

            return result
                .Select(x =>
                {
                    var confession = x.Object;
                    confession.Id = x.Key;
                    return confession;
                })
                .OrderByDescending(c => c.Votes)
                .ToList();
        }
    }
}
