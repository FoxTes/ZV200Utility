using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows;
using Microsoft.AppCenter;
using Microsoft.AppCenter.Analytics;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Prism.Ioc;
using Prism.Modularity;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;
using ZV200Utility.Core.Services;
using ZV200Utility.Modules.Setting;
using ZV200Utility.Modules.StatusRelays;
using ZV200Utility.Services.DeviceManager;
using ZV200Utility.Services.Notification;
using ZV200Utility.Services.SerialPortScanner;
using ZV200Utility.Views;

namespace ZV200Utility
{
    /// <summary>
    /// Представляет логику для App.xaml.
    /// </summary>
    public partial class App
    {
        /// <inheritdoc />
        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            SetCountryCode();
            AppCenter.Start("3d9569f9-aaaf-4212-994b-c70302679230",
                typeof(Analytics),
                typeof(Crashes));

            base.OnStartup(e);
        }

        /// <inheritdoc />
        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            AddConfiguration(containerRegistry);
            AddLogging(containerRegistry);

            containerRegistry.RegisterSingleton<INavigationJournal, NavigationJournal>();
            containerRegistry.RegisterSingleton<INotification, Notification>();

            containerRegistry.RegisterSingleton<ISerialPortScanner, SerialPortScanner>();
            containerRegistry.RegisterSingleton<IDeviceManager, DeviceManager>();
        }

        /// <inheritdoc />
        protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule<StatusRelaysModule>();
            moduleCatalog.AddModule<SettingModule>();
        }

        private static void AddConfiguration(IContainerRegistry containerRegistry)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetParent(AppContext.BaseDirectory).FullName)
                .AddJsonFile("appsettings.json", false)
                .Build();
            containerRegistry.RegisterInstance<IConfiguration>(configuration);
        }

        private static void AddLogging(IContainerRegistry containerRegistry)
        {
            var serilogLogger = Log.Logger = new LoggerConfiguration()
                .WriteTo.File("log\\log.log",
                    encoding: Encoding.UTF8,
                    restrictedToMinimumLevel: LogEventLevel.Verbose)
                .WriteTo.File("log\\logInfo.log",
                    encoding: Encoding.UTF8,
                    restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.File("log\\logError.log",
                    encoding: Encoding.UTF8,
                    restrictedToMinimumLevel: LogEventLevel.Error)
                .CreateLogger();
            var appLogger = new SerilogLoggerProvider(serilogLogger).CreateLogger("App");
            containerRegistry.RegisterInstance(appLogger);
        }

        private static void SetCountryCode()
        {
            var countryCode = RegionInfo.CurrentRegion.TwoLetterISORegionName;
            AppCenter.SetCountryCode(countryCode);
        }
    }
}
