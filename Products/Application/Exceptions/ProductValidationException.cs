using FluentValidation;
using FluentValidation.Results;

namespace Products.Application.Exceptions
{
    public class ProductValidationException : ValidationException
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public ProductValidationException(IEnumerable<ValidationFailure> errors) : base(errors)
        {
            Type = "product-validation-exception";
            Title = "Product validation exception.";
        }
    }
}
