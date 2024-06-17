using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Episode
    {
        public Episode()
        {
            Comments = new HashSet<Comment>();
            Views = new HashSet<View>();
        }

        public string EpisodeId { get; set; } = null!;
        public string? AnimeId { get; set; }
        public string? Title { get; set; }
        public int? Ep { get; set; }
        public DateTime PostingDate { get; set; }
        public string? VideoUrl { get; set; }

        public virtual Anime? Anime { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
