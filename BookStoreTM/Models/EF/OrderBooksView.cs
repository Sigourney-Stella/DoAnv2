using System.ComponentModel.DataAnnotations.Schema;

namespace BookStoreTM.Models.EF
{
    public class OrderBooksView
    {
        public int OrderId { get; set; }
        public string CodeOrder { get; set; }

        public DateTime OrderDate { get; set; }
        public decimal TotalMoney { get; set; }
        public string ReceiveName { get; set; }
        public string ReceiveAddress { get; set; }
        public string ReceivePhone { get; set; }
        public string Notes { get; set; }
        //khoá ngoại
        public string CustomerName { get; set; }
        public string TransactStatus { get; set; }
        public string PaymentName { get; set; }
    }
}
