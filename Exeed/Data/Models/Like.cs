namespace Exeed.Data.Models
{
    public class Like : IModel
    {
        public string? Id { get; set; }
        public virtual Account Account { get; set; }
        public virtual Desire Desire { get; set; }
    }
}
