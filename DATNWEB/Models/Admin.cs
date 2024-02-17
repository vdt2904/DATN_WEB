using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class Admin
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string PassWord { get; set; } = null!;
        public string Mail { get; set; } = null!;
        public int Auth { get; set; }
    }
}
