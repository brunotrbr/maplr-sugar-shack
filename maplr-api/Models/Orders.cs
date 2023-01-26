namespace maplr_api.Models
{
    public class Orders
    {
        public int Id { get; set; }
        public string ProductId { get; set; }
        public int Qty { get; set; }

        public Orders(int id, string productId, int qty)
        {
            Id = id;
            ProductId = productId;
            Qty = qty;
        }
    }
}
