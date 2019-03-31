namespace Demo.Import.Domain.Bills
{
    public class BillImport
    {
        public long Id { get; set; }
        public string InvoiceNo { get; set; }
        public string LineDetail { get; set;}

        public BillImport()
        {
        }

        public BillImport(long id, string invoiceNo, string lineDetail)
        {
            Id = id;
            InvoiceNo = invoiceNo;
            LineDetail = lineDetail;
        }

        public override string ToString()
        {
            return $"{InvoiceNo} {LineDetail}";
        }
    }
}
