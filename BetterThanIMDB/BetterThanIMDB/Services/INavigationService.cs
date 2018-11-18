using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BetterThanIMDB.Services
{
    /*public interface INavigationService
    {
        string CurrentPageKey { get; }

        void Configure(string pageKey, Type pageType);
        Task GoBackAsync();
        Task NavigateModalAsync(string pageKey, object parameter, bool animated = true);
        Task NavigateModalAsync(string pageKey, bool animated = true);
        Task NavigateAsync(string pageKey, object parameter, bool animated = true);
        Task NavigateAsync(string pageKey, bool animated = true);
    }*/

    public interface INavigationService
    {
        void Configure(string pageKey, Type pageType);
        Task GoBackAsync();
        Task NavigateAsync(string pageKey, object parameter, bool animated = true);
        Task NavigateAsync(string pageKey, bool animated = true);
    }
}
