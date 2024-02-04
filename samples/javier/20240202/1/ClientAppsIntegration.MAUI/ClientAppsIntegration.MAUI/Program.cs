//#if IOS || MACCATALYST || TIZEN
//namespace ClientAppsIntegration.MAUI
//{
//    public static class Program
//    {
//        // App EntryPoint
//        public static void Main(string[] args)
//        {
//            // Initialize HostApplicationBuilder
//            HostApplicationBuilder builder = Host.CreateApplicationBuilder();
//            builder.AddAppDefaults();

//            string scheme = builder.Environment.IsDevelopment() ? "http" : "https";
//            Uri endpoint = new($"{scheme}://apiservice");
//            builder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = endpoint);

//            IHost appHost = builder.Build(); 
//            Services = appHost.Services;
//            appHost.Start();

//            // Initialize .NET MAUI App
//#if IOS
//            ProgramiOS.Main(args);
//#endif

//#if MACCATALYST
//            ProgramMacCatalyst.Main(args);
//#endif

//#if TIZEN
//            ProgramTizen.Main(args);
//#endif

//            appHost.StopAsync().GetAwaiter().GetResult();

//            return;
//        }

//#if TIZEN
//        protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
//#endif
//        public static IServiceProvider? Services { get; private set; }

//        public static MauiApp CreateMauiApp()
//        {
//            return MauiProgram.CreateMauiApp();
//        }
//    }
//}
//#endif