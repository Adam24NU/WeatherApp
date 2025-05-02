using Microsoft.Maui.Controls;
using WeatherApp.Core.Tools;

namespace WeatherApp.Tools
{
    public class NavigationService : INavigationService
    {
        private readonly IServiceProvider _serviceProvider;

        // Page mappings dictionary
        private readonly Dictionary<string, Type> _pageMappings = new()
        {
            { "MainPage", typeof(WeatherApp.Views.MainPage) },
            { "LoginPage", typeof(WeatherApp.Views.LoginPage) },
            { "RegisterPage", typeof(WeatherApp.Views.RegisterPage) },
            { "AdminPage", typeof(WeatherApp.Views.AdminPage) },
            { "ScientistPage", typeof(WeatherApp.Views.ScientistPage) },
            { "ScientistMapPage", typeof(WeatherApp.Views.ScientistMapPage) },
            { "OpsManagerPage", typeof(WeatherApp.Views.OpsManagerPage) }
        };

        public NavigationService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task NavigateToAsync(string pageName)
        {
            // Check if the page name exists in the mappings
            if (!_pageMappings.TryGetValue(pageName, out var pageType))
            {
                throw new ArgumentException($"Page not found in mappings: {pageName}");
            }

            // Resolve the page using the service provider
            var page = _serviceProvider.GetService(pageType) as Page;
            if (page == null)
            {
                throw new InvalidOperationException($"Unable to resolve page: {pageName}");
            }

            // Perform navigation
            if (Application.Current?.MainPage?.Navigation != null)
            {
                await Application.Current.MainPage.Navigation.PushAsync(page);
            }
            else
            {
                throw new InvalidOperationException("Navigation stack is not available.");
            }
        }
    }
}

