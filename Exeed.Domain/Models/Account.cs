using Microsoft.AspNetCore.Identity;

namespace Exeed.Domain.Models
{
    public class Account : IModel
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public Role Role { get; set; }
        public virtual IdentityUser User { get; set; }
        public virtual Desire? Desire { get; set; }
        public virtual List<Like> Likes { get; set; }
    }
}
