using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Bill
    {
        public string Id { get; set; } = null!;
        public string? Userid { get; set; }
        public string? Description { get; set; }
        public DateTime? Createat { get; set; }
        public int? Ids { get; set; }
    }
}
