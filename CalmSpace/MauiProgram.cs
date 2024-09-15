using CalmSpace.Pages.AutumnPage;
using CalmSpace.Pages.SpringPage;
using CalmSpace.Pages.SummerPage;
using CalmSpace.Pages.WinterPage;
using Microsoft.Extensions.Logging;
using Plugin.Maui.Audio;

namespace CalmSpace
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<WinterPage>();
            builder.Services.AddTransient<SummerPage>();
            builder.Services.AddTransient<SpringPage>();
            builder.Services.AddTransient<AutumnPage>();
            return builder.Build();
        }
    }
}
