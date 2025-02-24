using Microsoft.Extensions.Logging;
using Pokedex.Services;

namespace Pokedex
{
    public static class MauiProgram
    {
        public static IServiceProvider serviceProvider;
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

            var services = new ServiceCollection();
            services.AddSingleton<IPokedexApi, PokedexApi>();
            serviceProvider = services.BuildServiceProvider();
#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // var services = new ServiceCollection();
            return builder.Build();
        }
    }
}
