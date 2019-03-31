using CSharpFunctionalExtensions;
using MediatR;

namespace Demo.Import.Application.Bills.Commands
{
    public class ExtractBillDetails : IRequest<Result>
    {
        public string InvoiceNo { get; }
        public string ImportFile { get; }

        public ExtractBillDetails(string invoiceNo, string importFile)
        {
            InvoiceNo = invoiceNo;
            ImportFile = importFile;
        }
    }
}
