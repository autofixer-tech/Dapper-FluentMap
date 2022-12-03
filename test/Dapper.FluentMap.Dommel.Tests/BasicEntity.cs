using System;

namespace Dapper.FluentMap.Dommel.Tests
{
    public abstract class BasicEntity
    {
        public DateTime CreatedAt { get; protected set; }

        public DateTime UpdatedAt { get; protected set; }
    }
}