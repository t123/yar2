namespace Yar.Data
{
    public class AppSettings
    {
        public static AppSettings Current;

        public string YarDatabase { get; set; }
        public string ApiKey { get; set; }

        public AppSettings()
        {
            Current = this;
        }
    }
}
