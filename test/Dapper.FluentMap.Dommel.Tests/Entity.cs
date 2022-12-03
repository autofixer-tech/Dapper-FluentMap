namespace Dapper.FluentMap.Dommel.Tests
{
    public abstract class Entity<TIdentity> : BasicEntity
    {
        public TIdentity Id { get; protected set; }
    }
}