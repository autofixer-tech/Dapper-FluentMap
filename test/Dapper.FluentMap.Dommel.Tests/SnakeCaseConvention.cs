using System.Linq;
using Dapper.FluentMap.Conventions;

namespace Dapper.FluentMap.Dommel.Tests;

public class SnakeCaseConvention : Convention
{
    public SnakeCaseConvention()
    {
        Properties().Configure(c => c.Transform(ToUnderscoreCase));
    }

    private static string ToUnderscoreCase(string @this)
    {
        if (string.IsNullOrEmpty(@this))
        {
            return @this;
        }

        return string.Concat(@this.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x : x.ToString()))
            .ToLowerInvariant();
    }
}