using Foundation;

namespace ClientAppsIntegration.MAUI.Platforms.iOS
{
    public abstract class MauiAspireUIApplicationDelegate : MauiUIApplicationDelegate
    {
        protected override MauiApp CreateMauiApp()
        {
            // Initialize HostApplicationBuilder
            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
            builder.AddAppDefaults();

            IHost appHost = builder.Build();
            Services = appHost.Services;
            appHost.Start();

            // Initialize .NET MAUI App
            var mauiApp = MauiProgram.CreateMauiApp();

            appHost.StopAsync().GetAwaiter().GetResult();

            return mauiApp;
        }
    }
}