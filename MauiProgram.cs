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

        // Flatten the SearchBar on each platform
        Microsoft.Maui.Handlers.SearchBarHandler.Mapper.AppendToMapping("FlatSearchBar", (handler, view) =>
        {
#if IOS || MACCATALYST
            var searchBar = handler.PlatformView;
            searchBar.BarTintColor = UIKit.UIColor.Clear;
            searchBar.BackgroundImage = new UIKit.UIImage();
            searchBar.SearchBarStyle = UIKit.UISearchBarStyle.Minimal;
            searchBar.SetSearchFieldBackgroundImage(new UIKit.UIImage(), UIKit.UIControlState.Normal);
            searchBar.BackgroundColor = UIKit.UIColor.Clear;

            // Strip background from all container subviews
            foreach (var outerSub in searchBar.Subviews)
            {
                outerSub.BackgroundColor = UIKit.UIColor.Clear;
                foreach (var innerSub in outerSub.Subviews)
                {
                    if (innerSub is not UIKit.UITextField)
                        innerSub.BackgroundColor = UIKit.UIColor.Clear;
                    innerSub.Layer.ShadowOpacity = 0;
                }
            }

            if (searchBar.SearchTextField is { } textField)
            {
                textField.BackgroundColor = UIKit.UIColor.FromRGBA(243, 244, 246, 255);
                textField.Layer.CornerRadius = 10;
                textField.Layer.MasksToBounds = true;
                textField.Layer.BorderWidth = 0;
                textField.Layer.ShadowOpacity = 0;
                textField.Font = UIKit.UIFont.SystemFontOfSize(15);
            }
#elif ANDROID
            var searchView = handler.PlatformView;
            int plateId = searchView.Context!.Resources!.GetIdentifier("android:id/search_plate", null, null);
            if (plateId != 0)
            {
                var plate = searchView.FindViewById(plateId);
                if (plate is not null)
                    plate.Background = null;
            }
#endif
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
