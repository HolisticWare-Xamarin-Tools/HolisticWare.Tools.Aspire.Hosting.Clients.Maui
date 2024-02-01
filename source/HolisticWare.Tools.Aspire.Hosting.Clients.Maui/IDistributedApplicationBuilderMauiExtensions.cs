namespace HolisticWare.Aspire.Hosting.Maui;

using Aspire.Hosting;

public static partial class 
                                        IDistributedApplicationBuilderMauiExtensions
{
    private static 
        IDistributedApplicationBuilder?
                                         Builder;

    private static 
        List<(string tfm, string device)> 
                                        devices = new List<(string tfm, string device)>();
     
    public static
        string?
                                        ProjectMaui
    {
        get;
        set;
    }

    public static
        IResourceBuilder<ProjectResource>
                                        AddProject
                                        (
                                            this IDistributedApplicationBuilder? builder,
                                            string name,
                                            string project,
                                            string framework,
                                            string device,
                                            int device_count
                                        )
    {
        ProjectMaui = project;
        MauiProjectSettings settings = new MauiProjectSettings()
                                                {
                                                    TargetFramework = framework,
                                                    DeviceName = device,
                                                    DeviceCount = device_count
                                                };

        if (framework.ToLower().Contains("android"))
        {
            settings.DeviceReady = false;

            System.Diagnostics.Process.Start
                                        (
                                            "/Users/moljac/Library/Android/sdk/emulator/emulator",
                                            settings.DeviceName
                                            // https://developer.android.com/studio/run/emulator-commandline
                                            + " " +
                                            "-no-cache" 
                                            + " " +
                                            "-gpu on"
                                            + " " +
                                            "-no-snapshot-load"
                                            + " " +
                                            "-no-boot-anim"
                                        );
        
            System.Threading.Thread.Sleep(10000);
        }
        else
        {
            settings.DeviceReady = true;
        }
 
        // IDistributedApplicationBuilderMauiExtensions.AddProject
        //                                                 (
        //                                                     builder,
        //                                                     name,
        //                                                     project,
        //                                                     settings
        //                                                 );

        return ProjectResourceBuilderExtensions.AddProject
                                                        (
                                                            builder,
                                                            name,
                                                            project
                                                        );
    }

    public static
        IDistributedApplicationBuilder
                                        BuildClient
                                        (
                                            this IDistributedApplicationBuilder? builder,
                                            string name,
                                            MauiProjectSettings settings 
                                        )
    {
        return builder;
    }

    public static
        IDistributedApplicationBuilder
                                        BuildClient
                                        (
                                            this IDistributedApplicationBuilder? builder, 
                                            string tfm, 
                                            string? device = null,
                                            int device_count = 1
                                        )
    {
        Builder = builder;
        devices.Add((tfm, device));
        
        return builder;
    }

    public static
        DistributedApplication
                                        BuildDistributedAppWithClientsMAUI
                                        (
                                            this IDistributedApplicationBuilder? builder
                                        )
    {
        Console.WriteLine("mc++");
        Console.WriteLine("     Inside Extension Method");
        Console.WriteLine("         RunMAUI");

        foreach (var r in Builder.Resources)
        {
            string n = r.Name;
        }

        // TODO: transform into Parallel.ForEach
        foreach(var device in devices)
        {
            Task.Run
                (
                    () =>
                    {


                        if (device.tfm.Contains("android"))
                        {
                            System.Diagnostics.Process.Start
                            (
                                "dotnet",
                                $"build {ProjectMaui} -f:net8.0-android -t:run"
                            );
                        }
                        else if (device.tfm.Contains("ios"))
                        {
                            System.Diagnostics.Process.Start
                            (
                                "dotnet",
                                // $"build {ProjectMaui} -f:net8.0-ios -t:run -r:iossimulator-arm64"
                                // $"build {ProjectMaui} -f:net8.0-ios -t:run -p:RuntimeIdentifier=ios-arm64 -p:_DeviceName=:v2:udid=017184FB-06E4-4C88-9662-13C1E2444486"
                                // $"build {ProjectMaui} -f:net8.0-ios -t:run -p:_DeviceName=:v2:udid=017184FB-06E4-4C88-9662-13C1E2444486"            
                                $"build {ProjectMaui} -f:net8.0-ios -t:run -p:_DeviceName=:v2:udid={device.device}"            
                            );                            
                        }
                        else if (device.tfm.Contains("maccatalyst"))
                        {
                            System.Diagnostics.Process.Start
                            (
                                "dotnet",
                                $"build {ProjectMaui} -f:net8.0-maccatalyst -t:run"
                            );
                        }
                        else
                        {

                        }
                    }
                );
        }

        // Parallel.Invoke
        // (
        //     () =>			// Param #1 - lambda expression
        //     {
        //     },
        //     delegate()		// Param #2 - in-line delegate
        //     {
        //         System.Diagnostics.Process.Start
        //         (
        //             "dotnet",
        //             // $"build {ProjectMaui} -f:net8.0-ios -t:run -r:iossimulator-arm64"
        //             // $"build {ProjectMaui} -f:net8.0-ios -t:run -p:RuntimeIdentifier=ios-arm64 -p:_DeviceName=:v2:udid=017184FB-06E4-4C88-9662-13C1E2444486"
        //             $"build {ProjectMaui} -f:net8.0-ios -t:run -p:_DeviceName=:v2:udid=017184FB-06E4-4C88-9662-13C1E2444486"            
        //         );
        //     },
        //     () =>			// Param #1 - lambda expression
        //     {
        //         System.Diagnostics.Process.Start
        //         (
        //             "dotnet",
        //             $"build {ProjectMaui} -f:net8.0-maccatalyst -t:run"
        //         );
        //     },
        //     () =>			// Param #1 - lambda expression
        //     {
        //         System.Diagnostics.Process.Start
        //         (
        //             "dotnet",
        //             // $"build {ProjectMaui} -f:net8.0-ios -t:run -r:iossimulator-arm64"
        //             $"build {ProjectMaui} -f:net8.0-ios -t:run -p:_DeviceName=:v2:udid=43A58A15-E4EA-4FDD-9DBD-5E8C16CBAF98"
                    
        //         );
        //     }
        //  );

        return builder.Build();   // Aspire Build() method- intercepted, can be called only once
    }

}
