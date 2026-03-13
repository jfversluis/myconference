using Microsoft.Extensions.Logging;
using MyConference.Services;
using MyConference.ViewModels;
using MyConference.Views;

namespace MyConference;

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

        // Services
        builder.Services.AddSingleton<ICacheService, CacheService>();
        builder.Services.AddSingleton<IFavoritesService, FavoritesService>();
        builder.Services.AddSingleton<ISessionizeService, SessionizeService>();
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddSingleton(Connectivity.Current);

        // ViewModels
        builder.Services.AddTransient<SessionsViewModel>();
        builder.Services.AddTransient<SessionDetailViewModel>();
        builder.Services.AddTransient<SpeakersViewModel>();
        builder.Services.AddTransient<SpeakerDetailViewModel>();
        builder.Services.AddTransient<FavoritesViewModel>();
        builder.Services.AddTransient<AboutViewModel>();

        // Pages
        builder.Services.AddTransient<SessionsPage>();
        builder.Services.AddTransient<SessionDetailPage>();
        builder.Services.AddTransient<SpeakersPage>();
        builder.Services.AddTransient<SpeakerDetailPage>();
        builder.Services.AddTransient<FavoritesPage>();
        builder.Services.AddTransient<AboutPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
