using System;
using System.Threading;
using System.Threading.Tasks;
using Demo.Import.Domain.Bills.Events;
using Demo.Messaging;
using MediatR;

namespace Demo.Import.ConsoleApp
{
    public class ProcessBillEventHandler:INotificationHandler<BillInvoiceExtracted>,INotificationHandler<BillDetailsExtracted>
    {
        public async Task Handle(BillInvoiceExtracted notification, CancellationToken cancellationToken)
        {
            var sendToUri = new Uri("rabbitmq://localhost/generate_invoice_queuex");
            var endPoint = await Program.BusControl.GetSendEndpoint(sendToUri);

            await endPoint.Send<IBillInvoiceExtracted>(notification);
        }

        public async Task Handle(BillDetailsExtracted notification, CancellationToken cancellationToken)
        {
            var sendToUri = new Uri("rabbitmq://localhost/generate_invoice_queuex");
            var endPoint = await Program.BusControl.GetSendEndpoint(sendToUri);

            await endPoint.Send<INotification>(notification);
        }
    }
}
