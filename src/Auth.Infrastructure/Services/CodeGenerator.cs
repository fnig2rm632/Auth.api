using Auth.Application.Interfaces.Service;

namespace Auth.Infrastructure.Services;

public class CodeGenerator : ICodeGenerator
{
    private static readonly Random Random = new();
    
    public string Generate()
    {
        var code = Random.Next(100000, 1000000);
        return code.ToString();
    }
}