using System;
using System.Collections.Generic;

namespace TDOffice_v2.DTO.Fiskalizacija
{
    public partial class FiskalniRacun
    {
        public string InvoiceNumber { get; set; }
        public string TIN { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? DateAndTimeOfPos { get; set; }
        public string Cashier { get; set; }
        public string BuyerTin { get; set; }
        public string BuyersCostCenter { get; set; }
        public string PosInvoiceNumber { get; set; }
        public List<PaymentItem> Payments { get; set; }
        public DateTime SDCTime_ServerTimeZone { get; set; }
        public string InvoiceCounter { get; set; }
        public string SignedBy { get; set; }
        public double TotalAmount { get; set; }
        public string TransactionType { get; set; }
        public string InvoiceType { get; set; }
        public List<Item> Items { get; set; }
        public List<TaxItem> TaxItems { get; set; }
    }
}
