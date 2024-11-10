

namespace Products.Application.Filters
{
    public class Filter
    {
        public double MinPrice { get; set; } = 0;
        public double MaxPrice { get; set; } = int.MaxValue;
        public Guid? SellerId { get; set; } = null;
    }
}
