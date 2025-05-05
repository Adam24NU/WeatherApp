using Microsoft.Maui;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Hosting;
using System;

namespace WeatherApp
{
    public static class ServiceHelper
    {
        public static T GetService<T>() =>
            Current.GetService<T>();

        public static IServiceProvider Current =>
            MauiApplication.Current.Services;  // ✅ This is the correct way
    }
}