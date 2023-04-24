namespace Falcon.Libraries.Common.Constants
{
    public class MessageConstant
    {
        public static class ValidatorMessageConstant
        {
            public const string FieldRequired = "{PropertyName} must be filled";
            public const string BeUniqueValue = "{PropertyName} with value: {PropertyValue} is already exists";
            public const string NotValidEmail = "{PropertyName} is not a valid email";
            public const string GreaterThan = "{PropertyName} must greater than {ComparisonValue}";
            public const string GreaterOrEqualThan = "{PropertyName} must greater or equal than {ComparisonValue}";
            public const string LessThan = "{PropertyName} must less than {ComparisonValue}";
            public const string LessOrEqualThan = "{PropertyName} must less or equal than {ComparisonValue}";
            public const string MaxLength = "{PropertyName} has a maximum of {MaxLength} characters";
        }

        public const string SuccessSave = "{0} has been saved";
        public const string FailedSave = "{0} failed to be save";
        public const string FailedValidate = "{0} failed to be validate";

        public const string StatusOk = "OK";
        public const string StatusBadRequest = "Bad Request";
        public const string StatusUnauthorized = "Unauthorized";
        public const string StatusForbidden = "Forbidden";
        public const string StatusNotFound = "Not Found";
        public const string StatusError = "Error";
    }
}
