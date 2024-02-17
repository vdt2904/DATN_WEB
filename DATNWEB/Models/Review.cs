using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Review
    {
        public int Id { get; set; }
        public string? AnimeId { get; set; }
        public string? UserId { get; set; }
        public int? Rating { get; set; }
        public string? Content { get; set; }
        public DateTime? Timestamp { get; set; }

        public virtual Anime? Anime { get; set; }
        public virtual User? User { get; set; }
    }
}
