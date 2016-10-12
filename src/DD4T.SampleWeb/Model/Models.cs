using DD4T.Core.Contracts.ViewModels;
using DD4T.ViewModels.Attributes;
using DD4T.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DD4T.SampleWeb.Model
{
    [PageViewModel(TemplateTitle = "Home Page")]
    public class Home : ViewModelBase, IHome
    {
        [PageId]
        public TcmUri Id { get; set; }

        [PageTitle]
        public string Title { get; set; }
    }

    [PageViewModel(TemplateTitle = "Content Page")]
    public class ContentPage : ViewModelBase, IHome
    {
        [PageId]
        public TcmUri Id { get; set; }

        [PageTitle]
        public string Title { get; set; }
    }

    public interface IHome : IViewModel
    {
        TcmUri Id { get; }
        string Title { get; }
    }
}