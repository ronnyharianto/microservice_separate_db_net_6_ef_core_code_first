using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain;
using FluentValidation;
using static Falcon.Libraries.Common.Constants.MessageConstants;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class VariantProductInputValidatior : AbstractValidator<VariantProductInput>
    {
        private readonly ApplicationDbContext _dbContext;

        public VariantProductInputValidatior(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(d => d.ProductId)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.ProductVariants)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);
        }

    }
}
