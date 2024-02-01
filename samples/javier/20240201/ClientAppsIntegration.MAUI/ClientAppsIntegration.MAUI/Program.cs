#if IOS || MACCATALYST
using ObjCRuntime;
using UIKit;
#endif

using System;
using System.Net.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Http;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ClientAppsIntegration.MAUI;


internal static class Program
{
    // This is the main entry point of the application.
    public static void Main(string[] args)
    {
        // -------------------------------------------------------------------------------------------------------------
        // Hosting
        //  start
        IHostApplicationBuilder builder = Host.CreateApplicationBuilder();

        builder.AddAppDefaults();
        
        string scheme = builder.Environment.IsDevelopment() ? "http" : "https";
        Uri endpoint = new($"{scheme}://apiservice");
        /*
        builder.Services.AddHttpClient<BasketServiceApiClient>(client => client.BaseAddress = endpoint);
        builder.Services
                .AddHttpServiceReference<CatalogServiceClient>("http://catalogservice", healthRelativePath: "readiness");

        builder.Services
                .AddSingleton<BasketServiceClient>()
                .AddGrpcServiceReference<Basket.BasketClient>("http://basketservice", failureStatus: HealthStatus.Degraded);
        */
        
        // WPF
        /*
        builder.Services.AddSingleton<App>();
        builder.Services.AddSingleton<MainPage>();
        */
        

        IHost app_host = builder.Build();

        // WinForms
        /*
        */
        Services = app_host.Services;
        app_host.Start();

        // WPF
        /*
        App app = app_host.Services.GetRequiredService<App>();
        Page page = app_host.Services.GetRequiredService<MainPage>();
        
        app_host.Start();
        app.Run(page);
       */

        //  stop
        // Hosting
        // -------------------------------------------------------------------------------------------------------------

        /*
            Original MAUI code:

            No Program.cs
        */

        #if ANDROID
        Microsoft.Maui.Hosting.MauiApp app_maui = CreateMauiApp();
        #endif

        #if IOS
        ProgramiOS.Main(args);
        #endif

        #if MACCATALYST
        ProgramMacCatalyst.Main(args);
        #endif
        
        #if TIZEN
        ProgramTizen.Main(args);
        #endif

        app_host.StopAsync().GetAwaiter().GetResult();

        return;
    }

    #if TIZEN
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
    #endif

    public static IServiceProvider Services { get; private set; } = default!;

    public static Microsoft.Maui.Hosting.MauiApp CreateMauiApp()
    {
        return MauiProgram.CreateMauiApp();
    }
}