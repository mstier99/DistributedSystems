namespace SourceGenerator.Models;

public sealed class Message
{
    public required int Id { get; init; }
    public required List<Location> Path { get; set; }
}
