using Shared.Domain.Base;

namespace General.Domain.Entities;

public class Attachment : Entity
{
    public string Title { get; set; }
    public string Path { get; set; }
}