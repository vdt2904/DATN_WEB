using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATNWEB.Models
{
    public partial class Season
    {
        public Season()
        {
            Animes = new HashSet<Anime>();
        }

        public string SeasonId { get; set; } = null!;
        [Required(ErrorMessage = "Tên mùa không được để trống!")]
        public string? SeasonName { get; set; }
        public string? ImageUrl { get; set; }
        public string? Information { get; set; }
        public DateTime? PostingDate { get; set; }

        public virtual ICollection<Anime> Animes { get; set; }
    }
}
