using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Demo.Import.Application.Bills.Commands.Handlers;
using Demo.Import.Domain.Bills.Events;
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
        public static List<object> Events;

        [OneTimeSetUp]
        public void Init()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.NUnitOutput()
                .CreateLogger();

            var services = new ServiceCollection()
                .AddMediatR(typeof(ExtractBillInvoiceHandler).Assembly,
                    typeof(TestEventHandler).Assembly);

            ServiceProvider = services.BuildServiceProvider();
            Events = new List<object>();
            Log.Debug("Tests initialized");
        }

        private class TestEventHandler :
            INotificationHandler<BillInvoiceExtracted>,
            INotificationHandler<BillDetailsExtracted>
        {
            public Task Handle(BillInvoiceExtracted notification, CancellationToken cancellationToken)
            {
                TestInitializer.Events.Add(notification);
                Log.Information($"{notification.InvoiceNo} - {notification.InvoiceLineDetail} ");
                return Task.CompletedTask;
            }

            public Task Handle(BillDetailsExtracted notification, CancellationToken cancellationToken)
            {
                TestInitializer.Events.Add(notification);
                Log.Information($"{notification.InvoiceNo} - {notification.LineDetails.Count} ");
                return Task.CompletedTask;
            }
        }
    }
}
