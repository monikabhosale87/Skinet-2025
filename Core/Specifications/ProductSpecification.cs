using System;
using Core.Entities;

namespace Core.Specifications;

public class ProductSpecification:BaseSpecification<Product>
{
    public ProductSpecification(ProductSpecParameters specParams) : base(x =>
    string.IsNullOrEmpty(specParams.Search)|| x.Name.ToLower().Contains(specParams.Search)&&
    (specParams.Brands.Count == 0 || specParams.Brands.Contains(x.Brand)) &&
    (specParams.Types.Count == 0 || specParams.Types.Contains(x.Type)))
    {
        ApplyPagination(specParams.PageSize * (specParams.PageIndex - 1), specParams.PageSize);
        switch (specParams.Sort)
        {
            case "priceAsc":
                AddOrderby(x => x.Price);
                break;
            case "priceDesc":
                AddOrderByDescending(x => x.Price);
                break;
            default:
                AddOrderby(x => x.Name);
                break;
        }
    }

//  public ProductSpecification(string? brand,string? type,string? sort):base(x =>
//  (string.IsNullOrWhiteSpace(brand)|| x.Brand== brand)&&
//  (string.IsNullOrWhiteSpace(type)|| x.Type==type))
}
