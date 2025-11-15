using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System;

namespace Domain.CourseProjectCleaning
{
    public class Payment
    {
        public int Id { get; set; }
        public int RequestId { get; set; }
        public string? CardNumberMasked { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public string? Status { get; set; }
        public string? TransactionId { get; set; }
    }
}