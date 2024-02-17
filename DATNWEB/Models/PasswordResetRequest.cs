using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class PasswordResetRequest
    {
        public int RequestId { get; set; }
        public string? UserId { get; set; }
        public string? Token { get; set; }
        public DateTime? RequestDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public int? IsUsed { get; set; }

        public virtual User? User { get; set; }
    }
}
