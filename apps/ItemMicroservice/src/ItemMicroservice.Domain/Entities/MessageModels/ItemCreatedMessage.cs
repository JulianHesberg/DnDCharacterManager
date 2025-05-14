using System;

namespace ItemMicroservice.Domain.Entities;

public class ItemCreatedMessage
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
