namespace ItemMicroservice.Domain.Entities.DTO;

public class ItemInputDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
