using System;
using System.Collections.Generic;

namespace DATNWEB.Models
{
    public partial class User
    {
        public User()
        {
            Comments = new HashSet<Comment>();
            PasswordResetRequests = new HashSet<PasswordResetRequest>();
            Reviews = new HashSet<Review>();
            UserSubscriptions = new HashSet<UserSubscription>();
            Views = new HashSet<View>();
        }

        public string UserId { get; set; } = null!;
        public string? Username { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public int? UserType { get; set; }

        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<PasswordResetRequest> PasswordResetRequests { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<UserSubscription> UserSubscriptions { get; set; }
        public virtual ICollection<View> Views { get; set; }
    }
}
