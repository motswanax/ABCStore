using Common.Pagination;

namespace Common.Requests.Products;

public class ProductFilterRequest : BaseFilter
{
    public int CategoryId { get; set; }
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
}
