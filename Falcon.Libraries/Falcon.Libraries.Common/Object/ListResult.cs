namespace Falcon.Libraries.Common.Object
{
    public class ListResult<T> : ServiceResult where T : IQueryable<T>
    {
        public ListResult() : base() { }

        public T? ListObj { get; set; }
    }
}
