namespace Exeed.Data.Models
{
    public class Winner : IModel
    {
        public string? Id { get; set; }
        public virtual Desire Desire { get; set; }
        public DateTime WonAt { get; set; }
        public Prize Prize { get; set; }
    }

    public enum Prize
    {
        None = 0,
        Ticket,
        Gift,
        Music
    }
}
