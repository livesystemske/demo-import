using System;
using System.IO;
using Demo.Import.Application.Bills.Commands;
using Demo.Import.Application.Bills.Commands.Handlers;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Demo.Import.ConsoleApp
{
    class Program
    {
        public static IServiceProvider ServiceProvider;
        public static IConfiguration Configuration;
        public static IBusControl BusControl;

        static void Main(string[] args)
        {
            Init();

            BusControl = ConfigureBus();
            BusControl.Start();

            Log.Information("app started !");

            ProcessBills();

            Console.ReadLine();

            BusControl.Stop();
        }

        static void Init()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            Configuration = builder.Build();

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .CreateLogger();

            Log.Information("Starting app..");
            var services = new ServiceCollection();
            services.AddMediatR(typeof(ExtractBillInvoiceHandler).Assembly,typeof(ProcessBillEventHandler).Assembly);
            ServiceProvider = services.BuildServiceProvider();
        }

        static IBusControl ConfigureBus()
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                cfg.UseSerilog();

                cfg.Host(new Uri(Configuration["RabbitMQ:uri"]), h =>
                {
                    h.Username(Configuration["RabbitMQ:username"]);
                    h.Password(Configuration["RabbitMQ:password"]);
                });
            });
        }

        static async void ProcessBills()
        {
            var mediator = ServiceProvider.GetService<IMediator>();

            var result = await mediator.Send(new ExtractBillInvoice("serviceimport.csv"));

            if (result.IsSuccess)
                Log.Information("Processed successfully");
            else
                Log.Error(result.Error);
        }
    }
}
