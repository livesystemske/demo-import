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
    public class ExtractBillDetailsHandler:IRequestHandler<ExtractBillDetails,Result>
    {
        private readonly IMediator _mediator;

        public ExtractBillDetailsHandler(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<Result> Handle(ExtractBillDetails request, CancellationToken cancellationToken)
        {
            Log.Information("Extracting Invoice Detail...");
            try
            {
                var imports = new List<BillImport>();

                using (var reader = new StreamReader(request.ImportFile))
                using (var csv = new CsvReader(reader))
                    imports = csv.GetRecords<BillImport>().ToList();

                if (!imports.Any())
                    throw new ArgumentException("File has no records!");

                var invoiceDetailsOnly = imports
                    .Where(x => x.InvoiceNo == request.InvoiceNo &&
                                !x.LineDetail.StartsWith("Amount:"))
                    .Distinct()
                    .Select(x=>x.LineDetail)
                    .ToList();

                if (!invoiceDetailsOnly.Any())
                    throw new ArgumentException($"Missing invoice [{request.InvoiceNo}] details records!");

                Log.Information($"Extracting Invoice [{request.InvoiceNo}] Details [Complete]");

                foreach (var invoice in invoiceDetailsOnly)
                    await _mediator.Publish(
                        new BillDetailsExtracted(request.InvoiceNo, invoiceDetailsOnly),
                        cancellationToken);

                return Result.Ok();
            }
            catch (Exception e)
            {
                Log.Error(e, "Extract Invoice Detail error!");
                return Result.Fail(e.Message);
            }
        }
    }
}
