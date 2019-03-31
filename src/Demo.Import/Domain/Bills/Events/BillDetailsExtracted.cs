using System.Collections.Generic;
using MediatR;

namespace Demo.Import.Domain.Bills.Events
{
    public class BillDetailsExtracted:INotification
    {
        public string InvoiceNo { get;  }
        public List<string>  LineDetails { get; }

        public BillDetailsExtracted(string invoiceNo, List<string> lineDetails)
        {
            InvoiceNo = invoiceNo;
            LineDetails = lineDetails;
        }
    }
}
