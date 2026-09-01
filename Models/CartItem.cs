namespace Next_Store.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public decimal Total { get { return Price * Quantity; } }
        public string Image { get; set; }
        public CartItem()
        {
            
        }
        public CartItem(ProductsModel products)
        {
            ProductId = products.Id;
            ProductName = products.Name;
            Price = products.Price;
            Quantity = 1;
            Image = products.Image;
          
        }
    }
}
