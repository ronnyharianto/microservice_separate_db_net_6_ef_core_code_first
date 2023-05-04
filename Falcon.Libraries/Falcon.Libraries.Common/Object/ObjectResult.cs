using Falcon.Libraries.Common.Enums;

namespace Falcon.Libraries.Common.Object
{
    public class ObjectResult<T> : ServiceResult where T : class
    {
        public ObjectResult(ServiceResultCode resultCode, string? message = null) : base(resultCode, message) { }

        public T? Obj { get; set; }
    }
}
