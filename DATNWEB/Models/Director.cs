using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DATNWEB.Models
{
    public partial class Director
    {
        public Director()
        {
            Animes = new HashSet<Anime>();
        }

        public string DirectorId { get; set; } = null!;
        [Required(ErrorMessage = "Tên Tác giả không được để trống!")]
        public string? DirectorName { get; set; }

        public virtual ICollection<Anime> Animes { get; set; }
    }
}
