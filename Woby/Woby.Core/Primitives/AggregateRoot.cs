using System.Collections.Immutable;

namespace Woby.Core.Signaling.Primitives
{
    public class AggregateRoot<Id> : Entity<Id>
    {

        private readonly ICollection<IDomainEvent> _domainEvents = new HashSet<IDomainEvent>();

        protected void GetDomainEvents() => _domainEvents.ToImmutableArray();

        protected void RaiseDomainEvent(IDomainEvent domainEvent) => 
            _domainEvents.Add(domainEvent);

        protected void RemoveDomainEvent(IDomainEvent domainEvent) =>
            _domainEvents.Remove(domainEvent);

    }
}
