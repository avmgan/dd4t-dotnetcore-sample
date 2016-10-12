using WTF4T.Modules.Navigation.Models;

namespace WTF4T.Moduels.Navigation.Providers
{
    public interface INavigationProvider
    {
        ISitemapItem GetAll(int levels = 0);

        ISitemapItem GetChildren(string requestUrlPath, int levels = 0, int startLevel = 0);

        ISitemapItem GetPath(string requestUrlPath);
    }
}