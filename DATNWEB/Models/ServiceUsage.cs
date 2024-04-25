using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class ServiceUsage
    {
        public int Id { get; set; }
        public string? PackageId { get; set; }
        public int? UsedTime { get; set; }
        public int? Price { get; set; }

        public virtual ServicePackage? Package { get; set; }
    }
}
