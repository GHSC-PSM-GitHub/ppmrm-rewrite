using System;
using AWS.Logger;
using AWS.Logger.SeriLog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace PPMRm.Web
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var logGroup = $"ppmrm-{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}";
            var configuration = new AWSLoggerConfig(logGroup);
            configuration.Region = "us-east-1";

            Log.Logger = new LoggerConfiguration()
#if DEBUG
                .MinimumLevel.Warning()
#else
                .MinimumLevel.Information()
#endif
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
                .MinimumLevel.Override("PPMRm", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Async(c => c.File("Logs/logs.txt"))
                .WriteTo.AWSSeriLog(configuration)
#if DEBUG
                .WriteTo.Async(c => c.Console())
                .WriteTo.AWSSeriLog(configuration)
#endif
                .CreateLogger();
            Log.Warning("Starting up");

            try
            {
                Log.Information("Starting web host.");
                CreateHostBuilder(args).Build().Run();
                return 0;
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly!");
                return 1;
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        internal static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(build =>
                {
                    build.AddJsonFile("appsettings.secrets.json", optional: true);
                })
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })
            .ConfigureLogging((context, loggingConfig) =>
            {
                loggingConfig.AddSerilog(Log.Logger);
            })
                .UseAutofac()
                .UseSerilog(Log.Logger);
    }
}
