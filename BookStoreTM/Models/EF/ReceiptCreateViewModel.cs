namespace BookStoreTM.Models.EF
{
    public class ReceiptCreateViewModel
    {
        public Receipt Receipt { get; set; }
        public List<ReceiptDetails> ReceiptDetails { get; set; }
    }
}
