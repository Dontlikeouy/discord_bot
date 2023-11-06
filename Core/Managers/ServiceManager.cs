using System;
using Microsoft.Extensions.DependencyInjection;

namespace Discord_bot.Core.Managers
{
    public static class ServiceManager
    {
        public static IServiceProvider Provider{ get; private set; }
        public static void SetProvider(ServiceCollection collection)
        => Provider = collection.BuildServiceProvider();
        public static T GetService<T>() where T : new()
        => Provider.GetRequiredService<T>();

        
    } 
}