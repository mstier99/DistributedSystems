using SourceGenerator.Models;

namespace SourceGenerator.Services;

public sealed class MessageGenerator
{
    public List<Message> GenerateMessages(int count)
    {
        var messages = new List<Message>();

        List<Location> path;

        for (int i = 0; i < count; ++i)
        {
            path = new List<Location> { new Location { Address = "Generator", Timestamp = DateTime.Now } };
        
            messages.Add(new Message { Id = i, Path = path });
        }

        return messages;
    }
}
