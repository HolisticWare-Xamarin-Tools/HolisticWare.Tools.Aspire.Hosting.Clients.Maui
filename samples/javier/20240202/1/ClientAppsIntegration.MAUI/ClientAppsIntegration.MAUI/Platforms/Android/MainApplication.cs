using Android.App;
using Android.Runtime;

namespace ClientAppsIntegration.MAUI
{
    [Application(
#if DEBUG
        UsesCleartextTraffic = true
#endif
    )]
    public class MainApplication : MauiAspireApplication
    {
        public MainApplication(IntPtr handle, JniHandleOwnership ownership)
            : base(handle, ownership)
        {
        }
    }
}