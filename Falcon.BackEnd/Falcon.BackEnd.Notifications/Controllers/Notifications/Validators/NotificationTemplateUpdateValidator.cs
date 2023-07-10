using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain;
using FluentValidation;
using static Falcon.Libraries.Common.Constants.MessageConstants;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class NotificationTemplateUpdateValidator : AbstractValidator<NotificationTemplateUpdate>
    {
        public NotificationTemplateUpdateValidator()
        {
            RuleFor(d => d.Title)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.Body)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);
        }
    }
}
