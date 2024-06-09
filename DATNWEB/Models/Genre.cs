using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATNWEB.Models
{
    public partial class Genre
    {
        public Genre()
        {
            FilmGenres = new HashSet<FilmGenre>();
        }

        public string GenreId { get; set; } = null!;
        [Required(ErrorMessage = "Tên Thể loại không được để trống!")]
        public string? GenreName { get; set; }
        public string? Infomation { get; set; }

        public virtual ICollection<FilmGenre> FilmGenres { get; set; }
    }
}
