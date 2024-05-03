using Microsoft.Extensions.Logging;
using Bookstore_OOP.Services;
using Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
using CommunityToolkit.Maui;

namespace Bookstore_OOP
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SignUpPage>();

            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<SignInPage>();

            builder.Services.AddTransient<AdminShell>();
            builder.Services.AddTransient<UserShell>();

            builder.Services.AddTransient<AddUserViewModel>();
            builder.Services.AddTransient<AddUserView>();

            builder.Services.AddTransient<AddBookViewModel>();
            builder.Services.AddTransient<AddBookView>();
           

#if DEBUG
builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}