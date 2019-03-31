using System.Linq;
using Demo.Import.Application.Bills.Commands;
using Demo.Import.Domain.Bills.Events;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Import.Tests.Bills
{
    [TestFixture]
    public class ExtractBillDetailsHandlerTests
    {
        private IMediator _mediator;

        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();
            TestInitializer.Events.Clear();

        }
        [Test]
        public void should_Extract_Invoice_Detail()
        {
            var command = new ExtractBillDetails("X020-01","serviceimport.csv");

            var result = _mediator.Send(command).Result;

            Assert.True(result.IsSuccess);
            Assert.True(TestInitializer.Events.OfType<BillDetailsExtracted>().Any());
        }

        [Test]
        public void should_Fail_Extract_Invoice_Detail()
        {
            var command = new ExtractBillDetails("X020-11","serviceimport.csv");

            var result = _mediator.Send(command).Result;

            Assert.True(result.IsFailure);
            Assert.False(TestInitializer.Events.OfType<BillDetailsExtracted>().Any());
        }
    }
}
