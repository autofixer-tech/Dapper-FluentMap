using System;

namespace Dapper.FluentMap.Dommel.Tests
{
    public static class EntityExtensions
    {
        public static bool IsNew(this BasicEntity @this) => @this.CreatedAt == default && @this.UpdatedAt == default;

        public static void SetPrincipalDates(this BasicEntity @this, DateTime date = default)
        {
            if (date == default)
            {
                date = DateTime.UtcNow;
            }

            @this.SetCreatedAt(date);
            @this.SetUpdatedAt(date);
        }

        public static void SetCreatedAt(this BasicEntity @this, DateTime createdAt = default)
        {
            var createdAtSetter = ObjectHelper.Setter<BasicEntity, DateTime>(_ => _.CreatedAt);
            if (createdAt == default)
            {
                createdAt = DateTime.UtcNow;
            }

            createdAtSetter.Invoke(@this, createdAt);
        }

        public static void SetUpdatedAt(this BasicEntity @this, DateTime updatedAt = default)
        {
            var updatedAtSetter = ObjectHelper.Setter<BasicEntity, DateTime>(_ => _.UpdatedAt);
            if (updatedAt == default)
            {
                updatedAt = DateTime.UtcNow;
            }

            updatedAtSetter.Invoke(@this, updatedAt);
        }
    }
}