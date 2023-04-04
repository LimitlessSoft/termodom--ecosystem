using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Termodom.Data.Entities.TDOffice_v2
{
    public class FiskalniRacun
    {
        public class Payment
        {
            public double Amount { get; set; }
            public string Type { get; set; }
        }
        public string InvoiceNumber { get; set; }
        public string TIN { get; set; }
        public string RequestedBy { get; set; }
        public DateTime? DateAndTimeOfPos { get; set; }
        public string Cashier { get; set; }
        public object BuyerTin { get; set; }
        public object BuyersCostCenter { get; set; }
        public string PosInvoiceNumber { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime SDCTime_ServerTimeZone { get; set; }
        public string InvoiceCounter { get; set; }
        public string SignedBy { get; set; }
        public double TotalAmount { get; set; }
        public string TransactionType { get; set; }
        public string InvoiceType { get; set; }
        public List<Payment> Payments
        {
            get
            {
                if (_payments == null)
                    _payments = JsonConvert.DeserializeObject<List<Payment>>(PaymentsRaw);

                return _payments;
            }
            set
            {
                PaymentsRaw = JsonConvert.SerializeObject(value);
                _payments = value;
            }
        }
        public string PaymentsRaw
        {
            get
            {
                return _paymentsRaw;
            }
            set
            {
                _paymentsRaw = value;
                _payments = null;
            }
        }

        private string _paymentsRaw { get; set; }
        private List<Payment> _payments { get; set; } = null;
    }
}
