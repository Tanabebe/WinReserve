using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WinReserve
{
    class Program
    {
        static IHostBuilder CreateDefaultBuilder()
        {
            IHostBuilder builder = Host.CreateDefaultBuilder()
                .ConfigureAppConfiguration((env, app) =>
                {
                    if (env.HostingEnvironment.IsDevelopment())
                    {
                        // 開発環境特有の処理
                    }
                    app.AddJsonFile("appsettings.json");
                })
                .ConfigureServices(services =>
                {
                    services.AddSingleton<Execute>();
                });
            return builder;
        }

        static void Main(string[] args)
        {
            var host = CreateDefaultBuilder().Build();
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider serviceProvider = scope.ServiceProvider;
            var execInsctance = serviceProvider.GetRequiredService<Execute>();
            execInsctance.DoRun();
            host.Run();
        }
    }
}
