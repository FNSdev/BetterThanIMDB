using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BetterThanIMDB.Services
{
    /*public class ViewNavigationService : INavigationService
    {
        #region fields
        private readonly object _sync = new object();
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private readonly Stack<NavigationPage> _navigationPageStack = new Stack<NavigationPage>();
        #endregion

        #region properties
        private NavigationPage CurrentNavigationPage => _navigationPageStack.Peek();
        public string CurrentPageKey
        {
            get
            {
                lock (_sync)
                {
                    if(CurrentNavigationPage?.CurrentPage == null)
                    {
                        return null;
                    }
                    var pageType = CurrentNavigationPage.CurrentPage.GetType();
                    return _pages.ContainsValue(pageType) ? _pages.First(page => page.Value == pageType).Key : null;
                }
            }
        }
        #endregion

        #region methods
        public void Configure(string pageKey, Type pageType)
        {
            lock(_sync)
            {
                if (_pages.ContainsKey(pageKey))
                {
                    _pages[pageKey] = pageType;
                }
                else
                {
                    _pages.Add(pageKey, pageType);
                }
            }
        }

        private Page GetPage(string pageKey, object parameter = null)
        {
            lock(_sync)
            {
                if (!_pages.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"Page {pageKey} is not in the dictionary. Configure it before using.");
                }

                var type = _pages[pageKey];
                ConstructorInfo constructor;
                object[] parameters;

                if(parameter == null)
                {
                    constructor = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(c => !c.GetParameters().Any());
                    parameters = new object[] { };
                }
                else
                {
                    constructor = type.GetTypeInfo().DeclaredConstructors.FirstOrDefault(
                        c => {
                            var p = c.GetParameters();
                            return p.Length == 1 && p[0].ParameterType == parameter.GetType(); 
                        });
                    parameters = new [] { parameter };
                }

                if(constructor == null)
                {
                    throw new InvalidOperationException($"There is no constructor found for page {pageKey}");
                }

                var page = constructor.Invoke(parameters) as Page;
                return page;
            }
        }

        public Page SetRootPage(string rootPageKey)
        {
            var rootPage = GetPage(rootPageKey);
            _navigationPageStack.Clear();
            var mainPage = new NavigationPage(rootPage);
            _navigationPageStack.Push(mainPage);
            return mainPage;
        }

        public async Task GoBackAsync() //?????????
        {
            var navigation = CurrentNavigationPage.Navigation;
            if (navigation.NavigationStack.Count > 1)
            {
                await CurrentNavigationPage.PopAsync();
                return;
            }

            if (_navigationPageStack.Count > 1)
            {
                _navigationPageStack.Pop();
                await CurrentNavigationPage.Navigation.PopModalAsync();
                return;
            }

            await CurrentNavigationPage.PopAsync();
        }

        public async Task NavigateAsync(string pageKey, object parameter, bool animated = true)
        {
            var page = GetPage(pageKey, parameter);
            await CurrentNavigationPage.Navigation.PushAsync(page, animated);
        }

        public async Task NavigateAsync(string pageKey, bool animated = true)
        {
            await NavigateAsync(pageKey, null, animated);
        }

        public async Task NavigateModalAsync(string pageKey, object parameter, bool animated = true)
        {
            var page = GetPage(pageKey, parameter);
            NavigationPage.SetHasNavigationBar(page, false);
            var modalNavigationPage = new NavigationPage(page);
            await CurrentNavigationPage.Navigation.PushAsync(page, animated);
            _navigationPageStack.Push(modalNavigationPage);
        }

        public async Task NavigateModalAsync(string pageKey, bool animated = true)
        {
            await NavigateModalAsync(pageKey, null, animated);
        }
        #endregion
    }*/
}
