using BlazorComponent;
using GaN.Translate;
using GaN.Translate.Impl.Parse;
using GaN.Translate.Impl.Translate;
using GaN.Translate.Interfaces;
using KristofferStrube.Blazor.FileSystemAccess;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;
using Photino.Blazor;

internal class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        var appBuilder = PhotinoBlazorAppBuilder.CreateDefault(args);

        appBuilder.RootComponents.Add<App>("#app");
        var service = appBuilder.Services;
        service.AddMasaBlazor(option =>
        {
            option.Locale = new Locale("zh-CN","en-US");
        });
        service.AddLogging(x =>
        {
            x.SetMinimumLevel(LogLevel.Information).AddJsonConsole().AddConsole();
        });

        service.AddSingleton<ITranslate, BaiduTranslate>()
            .AddSingleton<IInputParse, JsonParse>()
            .AddFileSystemAccessService()
            ;
        
        
        var app = appBuilder.Build();
        App.Provider = app.Services;
        
        app.MainWindow
            .SetTitle("Photino Blazor Sample");

        AppDomain.CurrentDomain.UnhandledException += (sender, error) => { };

        app.Run();
    }
}