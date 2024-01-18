using System.ComponentModel.DataAnnotations;

namespace Exeed.Data.Models
{
    public class Desire : IModel
    {
        public virtual string? Id { get; set; }
        [MaxLength(150)]
        public string Text { get; set; }
        public string? PhotoPath { get; set; }
        public bool? IsVerified { get; set; }
        public bool IsWon { get; set; }
        public virtual List<Like> Likes { get; set; }
    }
}
