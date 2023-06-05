using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain;
using FluentValidation;
using static Falcon.Libraries.Common.Constants.MessageConstants;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class ProductUpdateValidator : AbstractValidator<ProductUpdate>
    {
        public ProductUpdateValidator()
        {
            RuleFor(d => d.Name)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.Remark)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.Price)
                .GreaterThan(0).WithMessage(ValidatorMessageConstant.GreaterThan);
        }
    }
}
