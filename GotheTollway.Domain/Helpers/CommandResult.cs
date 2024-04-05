using CSharpFunctionalExtensions;

namespace GotheTollway.Domain.Helpers
{
    public record CommandResult(Result Result, object? Value = null) { }
}
