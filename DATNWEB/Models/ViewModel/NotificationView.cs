namespace DATNWEB.Models.ViewModel
{
    public class NotificationView
    {
        public string AnimeId { get; set; } = null!;
        public string? AnimeName { get; set; }
        public string? ImageVUrl { get; set; }
        public int? Episode { get; set; }
        public string? Title { get; set; }
        public DateTime? PostingDate { get; set; }
        public int? Permission { get; set; }
    }
}
