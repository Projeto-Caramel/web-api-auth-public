namespace Caramel.Pattern.Services.Domain.Entities.Models
{
    public interface IEntity<T>
        where T : IComparable, IEquatable<T>
    {
        T Id { get; set; }
    }
}
