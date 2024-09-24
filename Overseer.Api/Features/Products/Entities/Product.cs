using Overseer.Api.Features.Abstractions;

namespace Overseer.Api.Features.Products.Entities;

public class Product : Entity
{
    private Product(
        Guid id,
        string name,
        string description,
        decimal price)
        : base(id)
    {
        Name = name;
        Description = description;
        Price = price;
    }

    public string Name { get; private set; }

    public string Description { get; private set; }

    public decimal Price { get; private set; }

    public static Product Create(
        string name,
        string description,
        decimal price)
    {
        var product = new Product(Guid.NewGuid(), name, description, price);

        return product;
    }
}
