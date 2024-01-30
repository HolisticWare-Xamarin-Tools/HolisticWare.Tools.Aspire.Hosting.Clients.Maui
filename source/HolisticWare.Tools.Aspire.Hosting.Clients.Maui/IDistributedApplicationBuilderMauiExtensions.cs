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

        System.Diagnostics.Process.Start
        (
            "dotnet",
            $"build -f:net8.0-maccatalyst -t:run {ProjectMaui}"
        );
        System.Diagnostics.Process.Start
        (
            "dotnet",
            $"build -f:net8.0-ios -t:run {ProjectMaui}"
        );
        System.Diagnostics.Process.Start
        (
            "dotnet",
            $"build -f:net8.0-android -t:run {ProjectMaui}"
        );

        Parallel.ForEach
        (
            devices,
            tuple =>
            {
            }
        );

        return builder.Build();   // can be called only once
    }

}
