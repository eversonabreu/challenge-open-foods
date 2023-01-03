using Coodesh.Challenge.Domain.Entities.Base;
using Coodesh.Challenge.Domain.Enums;

namespace Coodesh.Challenge.Domain.Entities;

public sealed class Product : Entity
{
    public long Code { get; set; }

    public string BarCode { get; set; }

    public ProductStatus Status { get; set; }

    public DateTime? ImportedDate { get; set; }

    public string URL { get; set; }

    public string Name { get; set; }

    public string Quantity { get; set; }

    public string Categories { get; set; }

    public string Packaging { get; set; }

    public string Brands { get; set; }

    public string ImageURL { get; set; }
}