using Falcon.BackEnd.Products.Controllers.Products.Inputs;
using Falcon.BackEnd.Products.Domain;
using FluentValidation;
using static Falcon.Libraries.Common.Constants.MessageConstant;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class ProductInputValidator : AbstractValidator<ProductInput>
    {
        private readonly ApplicationDbContext _dbContext;

        public ProductInputValidator(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;

            RuleFor(d => d.Code)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired)
                .Must(BeUniqueCode).WithMessage(ValidatorMessageConstant.BeUniqueValue);

            RuleFor(d => d.Name)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.Price)
                .GreaterThan(0).WithMessage(ValidatorMessageConstant.GreaterThan);

            RuleFor(d => d.ProductVariants)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);
        }

        private bool BeUniqueCode(string name)
        {
            return _dbContext.Products.FirstOrDefault(x => x.Code.Equals(name)) == null;
        }
    }
}
