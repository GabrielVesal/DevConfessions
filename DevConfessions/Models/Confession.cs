namespace DevConfessions.Models
{
    public class Confession
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string? AuthorName { get; set; } = "";
        public string Content { get; set; } = "";
        public int Votes { get; set; } = 0;
        public DateTime DateCreated { get; set; } = DateTime.Now;
        //public string AuthorToken { get; set; } = "";
        public List<string> Hashtags { get; set; } = new();
    }
}
