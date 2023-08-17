namespace SourceGenerator.Models;

public sealed class Simulation
{
    public required int Id { get; set; }
    public string Description { get; set; } = "";
    public required DateTime Timestamp { get; set; }
    public required List<Message> Messages { get; set; }
}
