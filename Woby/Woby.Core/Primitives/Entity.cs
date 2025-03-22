namespace Woby.Core.Signaling.Primitives
{
    public abstract class Entity<IdType>
    {
        public IdType Id { get; init; }
    }
}
