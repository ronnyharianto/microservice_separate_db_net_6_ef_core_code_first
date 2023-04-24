using Falcon.Libraries.Common.Constants;

namespace Falcon.Libraries.Common.Object
{
    public class ServiceResult
    {
        public ServiceResult()
        {
            BadRequest(null);
        }
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Code { get; set; }
        public bool Succeeded { get; set; }
        public string? Message { get; set; }

        public void OK(string? message)
        {
            Code = 200;
            Succeeded = true;
            Message = message ?? MessageConstant.StatusOk;
        }
        public void BadRequest(string? message)
        {
            Code = 400;
            Succeeded = false;
            Message = message ?? MessageConstant.StatusBadRequest;
        }
        public void UnAuthorized(string? message)
        {
            Code = 401;
            Succeeded = false;
            Message = message ?? MessageConstant.StatusUnauthorized;
        }
        public void Forbidden(string? message)
        {
            Code = 403;
            Succeeded = false;
            Message = message ?? MessageConstant.StatusForbidden;
        }
        public void NotFound(string? message)
        {
            Code = 404;
            Succeeded = false;
            Message = message ?? MessageConstant.StatusNotFound;
        }
        public void Error(string? message)
        {
            Code = 500;
            Succeeded = false;
            Message = message ?? MessageConstant.StatusError;
        }
        public void TimeOut(string? message)
        {
            Code = 504;
            Succeeded = false;
            Message = message ?? MessageConstant.StatusTimeOut;
        }
    }
}
