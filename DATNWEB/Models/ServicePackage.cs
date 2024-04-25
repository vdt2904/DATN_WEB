using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class ServicePackage
    {
        public ServicePackage()
        {
            ServiceUsages = new HashSet<ServiceUsage>();
            UserSubscriptions = new HashSet<UserSubscription>();
        }

        public string PackageId { get; set; } = null!;
        public string? PackageName { get; set; }
        public string? Description { get; set; }
        public int? ValidityPeriod { get; set; }

        public virtual ICollection<ServiceUsage> ServiceUsages { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
    }
}
