using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Genre
    {
        public Genre()
        {
            FilmGenres = new HashSet<FilmGenre>();
        }

        public string GenreId { get; set; } = null!;
        public string? GenreName { get; set; }
        public string? Infomation { get; set; }

        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
    }
}
