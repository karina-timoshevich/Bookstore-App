using Microsoft.Extensions.Logging;
using Bookstore_OOP.Services;
using Bookstore_OOP.View;
using Bookstore_OOP.ViewModel;
namespace Bookstore_OOP
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

            builder.Services.AddSingleton<DatabaseService>();
            builder.Services.AddTransient<SignUpViewModel>();
            builder.Services.AddTransient<SignUpPage>();

            builder.Services.AddTransient<SignInViewModel>();
            builder.Services.AddTransient<SignInPage>();

            builder.Services.AddTransient<AdminShell>();
            builder.Services.AddTransient<UserShell>();

            builder.Services.AddTransient<AddUserViewModel>();
            builder.Services.AddTransient<AddUserView>();

#if DEBUG
builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}