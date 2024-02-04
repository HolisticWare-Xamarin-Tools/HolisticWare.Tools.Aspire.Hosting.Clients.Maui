namespace ClientAppsIntegration.MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var mauiAppBuilder = MauiApp.CreateBuilder();

            mauiAppBuilder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            var wrapperMauiAppBuilder = new WrapperMauiAppBuilder(mauiAppBuilder);

            wrapperMauiAppBuilder.AddAppDefaults();

#if DEBUG
            wrapperMauiAppBuilder.Logging.AddDebug();
#endif

            var scheme = wrapperMauiAppBuilder.Environment.IsDevelopment() ? "http" : "https";

            string baseAddress;
#if ANDROID
            baseAddress = $"{scheme}://10.0.2.2:5303";
#else
            baseAddress = $"{scheme}://localhost:5303";
#endif
            wrapperMauiAppBuilder.Services.AddHttpClient<WeatherApiClient>(client => client.BaseAddress = new(baseAddress));
            wrapperMauiAppBuilder.Services.AddSingleton<MainPage>();
            wrapperMauiAppBuilder.Services.AddSingleton<App>();

            MauiApp mauiApp = wrapperMauiAppBuilder.Build();

            return mauiApp;
        }
    }
}
