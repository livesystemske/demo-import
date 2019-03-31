using CSharpFunctionalExtensions;
using Demo.Import.Application.Bills.Commands;
using MediatR;
using NUnit.Framework;
using Microsoft.Extensions.DependencyInjection;

namespace Demo.Import.Tests.Bills
{
    [TestFixture]
    public class ExtractBillInvoiceHandlerTests
    {
        private IMediator _mediator;
        [SetUp]
        public void SetUp()
        {
            _mediator = TestInitializer.ServiceProvider.GetService<IMediator>();

        }
        [Test]
        public void should_Extract_Invoice()
        {
            var command = new ExtractBillInvoice("serviceimport.csv");

            var result = _mediator.Send(command).Result;

            Assert.True(result.IsSuccess);
        }

        [Test]
        public void should_Fail_Extract_Invoice()
        {
            var command = new ExtractBillInvoice("xxx.csv");

            var result = _mediator.Send(command).Result;

            Assert.True(result.IsFailure);
        }
    }
}
