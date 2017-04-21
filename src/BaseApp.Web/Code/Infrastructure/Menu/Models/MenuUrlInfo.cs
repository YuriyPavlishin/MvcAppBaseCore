namespace BaseApp.Web.Code.Infrastructure.Menu.Models
{
    public interface IMenuUrlInfo
    {
        string Url { get; }
        bool HasPermission { get; }
        bool IsCurrent { get; }
    }

    public class MenuUrlInfoRaw : IMenuUrlInfo
    {
        public string Url { get; }
        public bool HasPermission => true;
        public bool IsCurrent => false;

        public MenuUrlInfoRaw(string url)
        {
            Url = url;
        }
    }

    public class MenuUrlInfo : IMenuUrlInfo
    {
        public string Url { get; }
        public bool IsCurrent { get; }
        public bool HasPermission { get; }

        public MenuUrlInfo(string url, bool isCurrent, bool hasPermission)
        {
            Url = url;
            IsCurrent = isCurrent;
            HasPermission = hasPermission;
        }
    }
}