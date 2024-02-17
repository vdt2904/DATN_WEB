using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Anime
    {
        public Anime()
        {
            Episodes = new HashSet<Episode>();
            FilmGenres = new HashSet<FilmGenre>();
            Reviews = new HashSet<Review>();
        }

        public string AnimeId { get; set; } = null!;
        public string? AnimeName { get; set; }
        public string? ImageHUrl { get; set; }
        public string? ImageVUrl { get; set; }
        public string? BackgroundImageUrl { get; set; }
        public DateTime? BroadcastSchedule { get; set; }
        public string? Information { get; set; }
        public int? TotalEpisode { get; set; }
        public int? Permission { get; set; }
        public string? SeasonId { get; set; }
        public string? DirectorId { get; set; }

        public virtual Director? Director { get; set; }
        public virtual Season? Season { get; set; }
        public virtual ICollection<Episode> Episodes { get; set; }
        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
