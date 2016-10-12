using DD4T.ContentModel;
using DD4T.Core.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTF4T.Moduels.Navigation.Providers.Content
{
    public interface IContentProvider
    {
        IPage GetPage(string url);

        IPage GetPage(TcmUri pageId);

        DateTime GetPageLastPublishedDate(string url);

        string GetPageContent(string url);

        IComponentPresentation GetComponentPresentation(string componentId, string templateId = "");

        IViewModel BuildViewModel(TcmUri Id);

        IViewModel BuildViewModel(IModel model);

        IViewModel BuildViewModel(string url);

        //IViewModel BuildViewModel(TcmUri pageId);
    }
}