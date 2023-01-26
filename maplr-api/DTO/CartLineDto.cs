namespace maplr_api.DTO
{
    public class CartLineDto
    {
        public string ProductId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public double Price { get; set; }
        public int Qty { get; set; }

        public CartLineDto(string productId, string name, string image, double price, int qty)
        {
            ProductId = productId;
            Name = name;
            Image = image;
            Price = price;
            Qty = qty;
        }
    }
}
