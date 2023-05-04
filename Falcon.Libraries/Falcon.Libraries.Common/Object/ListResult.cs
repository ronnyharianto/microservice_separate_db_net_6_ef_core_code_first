using Falcon.Libraries.Common.Enums;

namespace Falcon.Libraries.Common.Object
{
    public class ListResult<T> : ServiceResult where T : IQueryable<T>
    {
        public ListResult(ServiceResultCode resultCode, string? message = null) : base(resultCode, message) { }

        public T? ListObj { get; set; }
    }
}
