namespace HolisticWare.Aspire.Hosting.Maui;

using System.Diagnostics;
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
                                            string[] tfms
                                        )
    {
        ProjectMaui = project;

        IResourceBuilder<ProjectResource>? resource_builder = default;

        if (builder != null)
        {
            resource_builder = ProjectResourceBuilderExtensions.AddProject
                                                                    (
                                                                        builder, 
                                                                        name, 
                                                                        project
                                                                    );                                                                    
        }
        else
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return resource_builder;
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
        
        List<(string tfm, string device)> devices_1 = new List<(string tfm, string device)>();
        List<(string tfm, string device)> devices_2 = new List<(string tfm, string device)>();

        foreach(var device in devices)
        {
            if (device.tfm.Contains("android"))
            {
                devices_2.Add(device);
            }
            else
            {
                devices_1.Add(device);
            }
        }

        // TODO: transform into Parallel.ForEach
        foreach(var device in devices_1)
        {
            if (device.tfm.Contains("ios"))
            {
                Trace.WriteLine($"Running iOS device.device = {device.device}");
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
                Trace.WriteLine($"Running MacCatalyst");
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

        // wait for Android devices to be ready
        Thread.Sleep(5000);
        foreach(var device in devices_2)
        {
            Trace.WriteLine($"Running Android device.device = {device.device}");
            // Process? process_clean = default;
            // process_clean = Process.Start
            //                             (
            //                                 "dotnet",
            //                                 $"clean  {ProjectMaui}"
            //                             );
            // process_clean.WaitForExit();

            Thread.Sleep(5000);
            // int exit_code = -1;
            // while (exit_code == 0)
            {                        
                Process? process_build = default;
                process_build = Process.Start
                                            (
                                                "dotnet",
                                                $"build {ProjectMaui} -f:net8.0-android -t:run"
                                            );
                process_build.WaitForExit();
                // exit_code = process_build.ExitCode;
            }
        }

        return builder.Build();   // Aspire Build() method- intercepted, can be called only once
    }

}
