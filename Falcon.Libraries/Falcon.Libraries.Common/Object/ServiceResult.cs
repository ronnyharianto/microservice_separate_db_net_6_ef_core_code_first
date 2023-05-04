using Falcon.Libraries.Common.Constants;
using Falcon.Libraries.Common.Enums;

namespace Falcon.Libraries.Common.Object
{
    public class ServiceResult
    {
        public ServiceResult(ServiceResultCode resultCode, string? message = null)
        {
            switch(resultCode)
            {
                case ServiceResultCode.Ok:
                    OK(message);
                    break;

                case ServiceResultCode.BadRequest:
                    BadRequest(message);
                    break;

                case ServiceResultCode.UnAuthorized:
                    UnAuthorized(message);
                    break;

                case ServiceResultCode.Forbidden:
                    Forbidden(message);
                    break;

                case ServiceResultCode.NotFound:
                    NotFound(message);
                    break;

                case ServiceResultCode.TimeOut:
                    TimeOut(message);
                    break;

                case ServiceResultCode.Error:
                    Error(message);
                    break;

                default: 
                    break;
            }
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Code { get; internal set; }
        public bool Succeeded { get; internal set; }
        public string? Message { get; set; }

        #region Public Methods
        public void OK(string? message)
        {
            Code = (int)ServiceResultCode.Ok;
            Succeeded = true;
            Message = message ?? MessageConstants.StatusOk;
        }
        public void BadRequest(string? message)
        {
            Code = (int)ServiceResultCode.BadRequest;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusBadRequest;
        }
        public void UnAuthorized(string? message)
        {
            Code = (int)ServiceResultCode.UnAuthorized;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusUnauthorized;
        }
        public void Forbidden(string? message)
        {
            Code = (int)ServiceResultCode.Forbidden;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusForbidden;
        }
        public void NotFound(string? message)
        {
            Code = (int)ServiceResultCode.NotFound;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusNotFound;
        }
        public void TimeOut(string? message)
        {
            Code = (int)ServiceResultCode.TimeOut;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusTimeOut;
        }
        public void Error(string? message)
        {
            Code = (int)ServiceResultCode.Error;
            Succeeded = false;
            Message = message ?? MessageConstants.StatusError;
        }
        #endregion
    }
}
