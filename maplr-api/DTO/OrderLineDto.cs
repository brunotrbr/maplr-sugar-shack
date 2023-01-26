﻿namespace maplr_api.DTO
{
    public class OrderLineDto
    {
        public string ProductId { get; set; }
        public int Qty { get; set; }
        public OrderLineDto(string productId, int qty)
        {
            ProductId = productId;
            Qty = qty;
        }
    }
}
