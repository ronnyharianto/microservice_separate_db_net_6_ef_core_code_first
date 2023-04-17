namespace Falcon.Libraries.Common.Object
{
    public class ObjectResult<T> : ServiceResult where T : class
    {
        public ObjectResult() : base() { }

        public T? Obj { get; set; }
    }
}
