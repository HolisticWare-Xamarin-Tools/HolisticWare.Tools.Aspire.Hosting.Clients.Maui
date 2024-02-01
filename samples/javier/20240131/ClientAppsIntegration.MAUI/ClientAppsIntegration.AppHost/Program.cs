using HolisticWare.Aspire.Hosting.Maui;

HolisticWare.Tools.Devices.Android.Emulator.Launch("nexus_9_api_33");
HolisticWare.Tools.Devices.Android.Emulator.Launch("Pixel_3a_API_34_extension_level_7_arm64-v8a");

System.Threading.Thread.Sleep(10000);

var builder = DistributedApplication.CreateBuilder(args);

var apiService = builder.AddProject<Projects.ClientAppsIntegration_ApiService>("apiservice");

// Register the client apps by project path as they target a TFM incompatible with the AppHost so can't be added as
// regular project references (see the AppHost.csproj file for additional metadata added to the ProjectReference to
// coordinate a build dependency though).

string project_maui = @"..\\ClientAppsIntegration.MAUI\\ClientAppsIntegration.MAUI.csproj";

builder
   .AddProject
         (
            "frontend_client_maui",
            project_maui,
            "net8.0-android",
            "Pixel_3a_API_34_extension_level_7_arm64-v8a",
            2
         )
         .WithReference(apiService);

builder
   .BuildClient
         (
            "net8.0-android",
            "Pixel_3a_API_34_extension_level_7_arm64-v8a",
            2
         )
    .BuildClient
         (
            "net8.0-android",
            "Nexus_9_API_33"    // tablet
            // default = 1
         )
    .BuildClient
         (
            "net8.0-ios",
            "017184FB-06E4-4C88-9662-13C1E2444486",
            2
         )
    .BuildClient
         (
            "net8.0-ios",
            "43A58A15-E4EA-4FDD-9DBD-5E8C16CBAF98"  // iPad
         )
    .BuildClient("maccatalyst")
    ;

builder
   // intercepting Build() to build/launch MAUI clients defined above
   // .Build()
   .BuildDistributedAppWithClientsMAUI()
   .Run();