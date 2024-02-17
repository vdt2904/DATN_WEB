using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Comment
    {
        public int Id { get; set; }
        public string? EpisodeId { get; set; }
        public string? UserId { get; set; }
        public DateTime? CommentDate { get; set; }
        public string? Comment1 { get; set; }

        public virtual Episode? Episode { get; set; }
        public virtual User? User { get; set; }
    }
}
