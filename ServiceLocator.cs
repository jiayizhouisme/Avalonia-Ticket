using GetStartedApp.Context;
using GetStartedApp.MyConverter;
using GetStartedApp.Services;
using GetStartedApp.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace GetStartedApp
{
    public class ServiceLocator
    {
        private readonly IServiceProvider _serviceProvider;

        public MainWindowViewModel MainWindowViewModel => _serviceProvider.GetService<MainWindowViewModel>();
        public IContainerExtension Container => _serviceProvider.GetService<IContainerExtension>();

        public ServiceLocator()
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<MainWindowViewModel>();
            serviceCollection.AddTransient<AppEFContext>();
            serviceCollection.AddTransient<IBlogService,BlogService>();

            _serviceProvider = serviceCollection.BuildServiceProvider();
        }
    }
}
