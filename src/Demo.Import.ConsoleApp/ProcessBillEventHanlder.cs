using System.Threading;
using System.Threading.Tasks;
using Demo.Import.Domain.Bills.Events;
using MediatR;

namespace Demo.Import.ConsoleApp
{
    public class ProcessBillEventHandler:INotificationHandler<BillInvoiceExtracted>,INotificationHandler<BillDetailsExtracted>
    {
        public Task Handle(BillInvoiceExtracted notification, CancellationToken cancellationToken)
        {
            Program.BusControl.Publish<BillInvoiceExtracted>(notification);
            return Task.CompletedTask;
        }

        public Task Handle(BillDetailsExtracted notification, CancellationToken cancellationToken)
        {
            Program.BusControl.Publish<BillInvoiceExtracted>(notification);
            return Task.CompletedTask;
        }
    }
}
