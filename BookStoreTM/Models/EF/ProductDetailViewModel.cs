namespace BookStoreTM.Models.EF
{
    public class ProductDetailViewModel
    {
        public int OrderDetailsId { get; set; }
        public string ProductName { get; set; }
        public string CatName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal TotalMoney { get; set; }
        public int OrderId { get; set; }
        public string Description { get; set; }
        public string Images { get; set; }
    }

}
