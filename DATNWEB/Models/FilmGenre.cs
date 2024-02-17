using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class FilmGenre
    {
        public int Id { get; set; }
        public string? AnimeId { get; set; }
        public string? GenreId { get; set; }

        public virtual Anime? Anime { get; set; }
        public virtual Genre? Genre { get; set; }
    }
}
