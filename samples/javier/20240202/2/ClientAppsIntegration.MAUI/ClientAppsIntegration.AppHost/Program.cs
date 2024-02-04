using HolisticWare.Aspire.Hosting.Maui;

HolisticWare.Tools.Devices.Android.Emulator.Launch("pixel_5_-_api_34");

// TODO: Get notified when emulators is running and ready.
System.Threading.Thread.Sleep(20000);

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ClientAppsIntegration_ApiService>("apiservice");

// Register the client apps by project path as they target a TFM incompatible with the AppHost so can't be added as
// regular project references (see the AppHost.csproj file for additional metadata added to the ProjectReference to
// coordinate a build dependency though).

//builder.AddProject("mauiclient", "../ClientAppsIntegration.MAUI/ClientAppsIntegration.MAUI.csproj")
//    .WithReference(apiService);

string project_maui = @"..\\ClientAppsIntegration.MAUI\\ClientAppsIntegration.MAUI.csproj";

builder
   .AddProject
         (
            "frontend_client_maui",
            project_maui,
            "net8.0-android",
            "pixel_5_-_api_34",
            2
         )
         .WithReference(apiService);

builder
   .BuildClient
         (
            "net8.0-android",
            "pixel_5_-_api_34",
            2
         );

builder
   // intercepting Build() to build/launch MAUI clients defined above
   //.Build()
   .BuildDistributedAppWithClientsMAUI()
   .Run();