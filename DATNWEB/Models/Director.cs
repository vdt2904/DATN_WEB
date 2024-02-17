using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Director
    {
        public Director()
        {
            Animes = new HashSet<Anime>();
        }

        public string DirectorId { get; set; } = null!;
        public string? DirectorName { get; set; }

        public virtual ICollection<Anime> Animes { get; set; }
    }
}
