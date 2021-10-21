using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using FluentMigrator.Runner;

namespace BaseAcoes
{
    public class Startup
    {
        public static void ConfigureServices(IServiceCollection services,
            string[] args)
        {
            Console.WriteLine("Configurando recursos...");

            services.AddLogging(configure => configure.AddConsole());

            services.AddFluentMigratorCore()
                .ConfigureRunner(cfg => cfg
                    .AddSQLite()
                    .WithGlobalConnectionString(args[0])
                    .ScanIn(typeof(Startup).Assembly).For.Migrations()
                )
                .AddLogging(cfg => cfg.AddFluentMigratorConsole());

            services.AddTransient<ConsoleApp>();
        }
    }
}