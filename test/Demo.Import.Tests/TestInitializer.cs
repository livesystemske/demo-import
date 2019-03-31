using System;
using Demo.Import.Application.Bills.Commands.Handlers;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Demo.Import.Tests
{
    [SetUpFixture]
    public class TestInitializer
    {
        public static IServiceProvider ServiceProvider;

        [OneTimeSetUp]
        public void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NUnitOutput()
                .CreateLogger();

            var services = new ServiceCollection()
                .AddMediatR(typeof(ExtractBillInvoiceHandler).Assembly);

            ServiceProvider = services.BuildServiceProvider();

            Log.Debug("Tests initialized");
        }
    }
}
