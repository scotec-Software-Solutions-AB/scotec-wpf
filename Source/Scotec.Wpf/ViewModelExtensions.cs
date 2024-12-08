using Microsoft.Extensions.DependencyInjection;

namespace Scotec.Wpf;

public static class ViewModelExtensions
{
    public static IServiceCollection AddDataTemplateSelector(this IServiceCollection services)
    {
        return services.AddSingleton<GlobalViewModelTemplateSelector>()
                       .AddTransient<ViewModelTemplateSelector>();
    }
}
