
namespace oyster.web.host
{
    public abstract class TimHost
    {
        protected TimHost()
        {
            if (Instance == null)
                Instance = this;
        }
        public static TimHost Instance { get; protected set; }

        public virtual Response Execute(Request request)
        {
            var process = new TimProcess(this, request);
            process.Process();
            return process.Response;
        }

        public abstract TimTheme GetTheme(Request request);
    }
}
