namespace Products.Application.Exceptions
{
    public class ProductNotFoundException : ProductBaseException
    {
        public ProductNotFoundException(string instance)
        {
            Type = "product-not-found-exception";
            Title = "Product not found exception.";
            Detail = "Required product cannot be found.";
            Instance = instance;
        }
    }
}
