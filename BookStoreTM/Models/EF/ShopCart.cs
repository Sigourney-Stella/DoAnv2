namespace BookStoreTM.Models.EF
{
    public class ShopCart
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string Alias { get; set; }
        public string CategoryName { get; set; }
        public string ProductImg { get; set; }
        public int Quantity { get; set; }
        public int MaxQuantity { get; set; }
        public int SoLuong { get; set; }
        public decimal Price { get; set; }
        public decimal PriceSale { get; set; }
        public decimal TotalPrice { get; set; }
    }

}
