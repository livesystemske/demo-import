using Demo.Messaging;
using MediatR;

namespace Demo.Import.Domain.Bills.Events
{
    public class BillInvoiceExtracted:IBillInvoiceExtracted, INotification
    {
        public string InvoiceNo { get;  }
        public string InvoiceLineDetail { get;  }

        public BillInvoiceExtracted(string invoiceNo, string invoiceLineDetail)
        {
            InvoiceNo = invoiceNo;
            InvoiceLineDetail = invoiceLineDetail;
        }
    }
}
