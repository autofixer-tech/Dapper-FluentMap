using Dapper.FluentMap.Conventions;

namespace Dapper.FluentMap.Dommel.Tests;

public class SnakeCaseConvention : Convention
{
    public SnakeCaseConvention()
    {
        Properties().Configure(c => c.Transform(_ => _.ToSnakeCase()));
    }
}