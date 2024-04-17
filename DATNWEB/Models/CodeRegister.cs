using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class CodeRegister
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Token { get; set; }
        public DateTime? SentDate { get; set; }
    }
}
