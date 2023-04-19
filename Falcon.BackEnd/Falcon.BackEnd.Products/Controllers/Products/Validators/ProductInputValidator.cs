using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using FluentValidation;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class ProductInputValidator : AbstractValidator<ProductInput>
    {
        public ProductInputValidator()
        {
            RuleFor(d => d.Price).GreaterThan(0);
        }
    }
}
