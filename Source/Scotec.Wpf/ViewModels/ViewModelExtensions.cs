using Microsoft.Extensions.DependencyInjection;

namespace Scotec.Wpf.ViewModels;

public static class ViewModelExtensions
{
    public static IServiceCollection AddViewModelTemplateSelector(this IServiceCollection services)
    {
        return services.AddActivatedSingleton<GlobalViewModelTemplateSelector>()
                       .AddTransient<ViewModelTemplateSelector>();
    }
}
