

namespace Products.Application.Filters
{
    public class Filter
    {
        public int MinPrice { get; set; } = 0;
        public int MaxPrice { get; set; } = int.MaxValue;
        public Guid? SellerId { get; set; } = null;
    }
}
