using Falcon.BackEnd.Notifications.Controllers.Notifications.Inputs;
using Falcon.BackEnd.Notifications.Domain;
using FluentValidation;
using static Falcon.Libraries.Common.Constants.MessageConstants;

namespace Falcon.BackEnd.Products.Controllers.Products.Validators
{
    public class UserNotificationInputValidator : AbstractValidator<UserNotificationInput>
    {
        public UserNotificationInputValidator()
        {
            RuleFor(d => d.UserId)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.FcmToken)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);

            RuleFor(d => d.UserName)
                .NotEmpty().WithMessage(ValidatorMessageConstant.FieldRequired);
        }
    }
}
