using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CsvHelper;
using CSharpFunctionalExtensions;
using Demo.Import.Domain.Bills;
using Demo.Import.Domain.Bills.Events;
using MediatR;
using Serilog;

namespace Demo.Import.Application.Bills.Commands.Handlers
{
    public class ExtractBillInvoiceHandler : IRequestHandler<ExtractBillInvoice, Result>
    {
        private readonly IMediator _mediator;

        public ExtractBillInvoiceHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(ExtractBillInvoice request, CancellationToken cancellationToken)
        {
            Log.Information("Extracting Invoice...");
            try
            {
                var imports = new List<BillImport>();

                using (var reader = new StreamReader(request.ImportFile))
                using (var csv = new CsvReader(reader))
                    imports = csv.GetRecords<BillImport>().ToList();

                if (!imports.Any())
                    throw new ArgumentException("File has no records!");

                var invoiceOnly = imports.Where(x => x.LineDetail.StartsWith("Amount:"))
                    .Distinct()
                    .ToList();

                if (!invoiceOnly.Any())
                    throw new ArgumentException("Missing invoice records!");

                Log.Information("Extracting Invoice [Complete]");

                foreach (var invoice in invoiceOnly)
                    await _mediator.Publish(
                        new BillInvoiceExtracted(invoice.InvoiceNo, invoice.LineDetail),
                        cancellationToken);

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, "Extract Invoice error!");
                return Result.Fail(e.Message);
            }
        }
    }
}
