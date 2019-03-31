using CSharpFunctionalExtensions;
using MediatR;

namespace Demo.Import.Application.Bills.Commands
{
    public class ExtractBillInvoice:IRequest<Result>
    {
        public string ImportFile { get; }

        public ExtractBillInvoice(string importFile)
        {
            ImportFile = importFile;
        }
    }
}
