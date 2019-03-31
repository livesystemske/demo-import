using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using MediatR;

namespace Demo.Import.Application.Bills.Commands.Handlers
{
    public class ExtractBillDetailsHandler:IRequestHandler<ExtractBillDetails,Result>
    {
        public Task<Result> Handle(ExtractBillDetails request, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }
    }
}
