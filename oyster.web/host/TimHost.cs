
namespace oyster.web.host
{
    public abstract class TimHost
    {
        public virtual Response Execute(Request request)
        {
            var process = new TimProcess(this, request);
            process.Process();
            return process.Response;
        }

        public abstract TimTheme GetTheme(Request request);
        public abstract TimTheme GetTheme(string themeName);
    }
}
