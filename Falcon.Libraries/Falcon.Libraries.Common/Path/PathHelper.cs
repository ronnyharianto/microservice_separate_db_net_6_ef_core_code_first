namespace Falcon.Libraries.Common.Path
{
    public class PathHelper : IPathHelper
    {
        public string CurrentPath(string path)
        {
            return System.IO.Path.Combine(Directory.GetCurrentDirectory(), path);

        }
    }
}
