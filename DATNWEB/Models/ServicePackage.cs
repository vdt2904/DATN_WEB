using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class ServicePackage
    {
        public ServicePackage()
        {
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        public string PackageId { get; set; } = null!;
        public string? PackageName { get; set; }
        public string? Description { get; set; }
        public double? Price { get; set; }
        public int? ValidityPeriod { get; set; }

        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
