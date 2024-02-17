using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class View
    {
        public int Id { get; set; }
        public string? EpisodeId { get; set; }
        public string? UserId { get; set; }
        public DateTime? ViewDate { get; set; }
        public TimeSpan? Duration { get; set; }
        public int? IsView { get; set; }

        public virtual Episode? Episode { get; set; }
        public virtual User? User { get; set; }
    }
}
