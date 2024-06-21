using MAUI.Translate.Rcl.Impl.Parse;
using MAUI.Translate.Rcl.Impl.Translate;
using MAUI.Translate.Rcl.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace MAUI.Translate.Rcl.Extensions;

public static class CommonServiceRegister
{
    public static IServiceCollection AddCommonService(this IServiceCollection services)
    {
        services.AddSingleton<ITranslate, BaiduTranslate>()
            .AddSingleton<IInputParse, JsonParse>()
            ;
        return services;
    }
}