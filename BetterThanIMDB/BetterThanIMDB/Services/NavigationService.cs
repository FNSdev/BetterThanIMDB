using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace BetterThanIMDB.Services
{
    public class NavigationService : INavigationService
    {
        private readonly object _sync = new object();
        private readonly Dictionary<string, Type> _pages = new Dictionary<string, Type>();
        private NavigationPage _currentNavigationPage;

        public void Configure(string pageKey, Type pageType)
        {
            lock (_sync)
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
            lock (_sync)
            {
                if (!_pages.ContainsKey(pageKey))
                {
                    throw new ArgumentException($"Page {pageKey} is not in the dictionary. Configure it before using.");
                }

                var type = _pages[pageKey];
                ConstructorInfo constructor;
                object[] parameters;

                if (parameter == null)
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
                    parameters = new[] { parameter };
                }

                if (constructor == null)
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
            var mainPage = new NavigationPage(rootPage);
            _currentNavigationPage = mainPage;
            return mainPage;
        }

        public async Task NavigateAsync(string pageKey, object parameter, bool animated = true)
        {
            var page = GetPage(pageKey, parameter);
            await _currentNavigationPage.PushAsync(page, animated);
        }

        public async Task NavigateAsync(string pageKey, bool animated = true)
        {
            await NavigateAsync(pageKey, null, animated);
        }

        public async Task GoBackAsync()
        {
            var navigation = _currentNavigationPage.Navigation;
            if (navigation.NavigationStack.Count > 1)
            {
                await _currentNavigationPage.PopAsync();
                return;
            }

        }
    }
}
