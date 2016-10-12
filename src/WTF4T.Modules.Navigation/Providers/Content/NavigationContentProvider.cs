using DD4T.ContentModel;
using DD4T.ContentModel.Contracts.Configuration;
using DD4T.ContentModel.Contracts.Logging;
using DD4T.ContentModel.Factories;
using DD4T.Core.Contracts.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WTF4T.Moduels.Navigation.Providers.Content
{
    public class NavigationContentProvider : IContentProvider
    {
        private readonly ILogger _logger;
        private readonly IPageFactory _pageFactory;
        private readonly IViewModelFactory _viewModelFactory;
        private readonly IComponentPresentationFactory _componentPresentationFactory;
        private readonly IDD4TConfiguration _configuration;
        //private readonly IExtendedPublicationResolver _publicationResolver;

        public NavigationContentProvider(IPageFactory pageFactory,
                                   IComponentPresentationFactory componentPresentationFactory,
                                   IViewModelFactory viewModelFactory,
                                   ILogger logger,
                                   //IExtendedPublicationResolver publicationResolver,
                                   IDD4TConfiguration configuration)
        {
            pageFactory.ThrowIfNull(nameof(pageFactory));
            viewModelFactory.ThrowIfNull(nameof(viewModelFactory));
            logger.ThrowIfNull(nameof(logger));
            componentPresentationFactory.ThrowIfNull(nameof(componentPresentationFactory));
            configuration.ThrowIfNull(nameof(configuration));
            //publicationResolver.ThrowIfNull(nameof(publicationResolver));

            //this._publicationResolver = publicationResolver;
            this._pageFactory = pageFactory;
            this._viewModelFactory = viewModelFactory;
            this._componentPresentationFactory = componentPresentationFactory;
            this._logger = logger;
            this._configuration = configuration;
        }

        public IComponentPresentation GetComponentPresentation(string componentId, string templateId = "")
        {
            return _componentPresentationFactory.GetComponentPresentation(componentId, templateId);
        }

        public bool TryGetComponentPresentation(out IComponentPresentation component, string componentId, string templateId = "")
        {
            return _componentPresentationFactory.TryGetComponentPresentation(out component, componentId, templateId);
        }

        public IPage GetPage(string url)
        {
            _logger.Debug("GetPage - Processing request for page: {0}", url);
            IPage page;
            if (_pageFactory.TryFindPage(url.ParseUrl(), out page))
            {
                return page;
            }

            return null;
        }

        public IPage GetPage(TcmUri pageId)
        {
            var page = _pageFactory.GetPage(pageId.ToString());
            return page;
        }

        public string GetPageContent(string url)
        {
            _logger.Debug("GetPageContent - Processing request for page: {0}", url);
            string pageContent;
            if (_pageFactory.TryFindPageContent(url.ParseUrl(), out pageContent))
            {
                return pageContent;
            }
            return pageContent;
        }

        public DateTime GetPageLastPublishedDate(string url)
        {
            return _pageFactory.GetLastPublishedDateByUrl(url);
        }

        public IViewModel BuildViewModel(IModel model)
        {
            return _viewModelFactory.BuildViewModel(model);
        }

        public IViewModel BuildViewModel(string url)
        {
            var page = GetPage(url);
            return BuildViewModel(page);
        }

        public IViewModel BuildViewModel(TcmUri Id)
        {
            IViewModel model = null;
            if (Id.ItemTypeId == 64)
            {
                model = BuildViewModel(GetPage(Id));
            }
            else
            {
                IComponentPresentation component;
                if (TryGetComponentPresentation(out component, Id.ToString()))
                {
                    model = BuildViewModel(component);
                }
            }
            return model;
        }
    }
}