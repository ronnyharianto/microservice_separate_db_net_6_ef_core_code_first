using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain;
using FluentValidation;
using static Falcon.Libraries.Common.Constants.MessageConstants;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class NotificationTemplateInputValidator : AbstractValidator<NotificationTemplateInput>
    {
        public NotificationTemplateInputValidator()
        {
            RuleFor(d => d.Title)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.Body)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.Code)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);
        }
    }
}
