using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class UserSubscription
    {
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string? PackageId { get; set; }
        public DateTime? SubscriptionDate { get; set; }
        public DateTime? ExpirationDate { get; set; }

        public virtual ServicePackage? Package { get; set; }
        public virtual User? User { get; set; }
    }
}
