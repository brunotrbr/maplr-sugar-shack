using maplr_api.DTO;
using maplr_api.Interfaces;

namespace maplr_api.BusinessLayers
{
    public class OrdersBL
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public OrdersBL(IOrderRepository orderRepository, IProductRepository productRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _productRepository = productRepository;
            _cartRepository = cartRepository;
        }

        public Task<List<string>> ValidateOrder(List<OrderLineDto> ordersDto)
        {
            return Task.Run(() =>
            {
                List<string> errors = new List<string>();
                if (ordersDto.Count <= 0)
                {
                    errors.Add($"Empty order");
                    return errors;
                }

                var repOrdersDto = _orderRepository.Get().Result;
                var cartsDto = _cartRepository.Get().Result;
                var productsDto = _productRepository.Get(0).Result;

                if (repOrdersDto.Any())
                {
                    var repeateds = (from x in repOrdersDto
                                 join y in ordersDto on x.ProductId equals y.ProductId
                                 select x).Select(x => x.ProductId);
                    foreach (string productId in repeateds)
                    {
                        errors.Add($"ProductId {productId} already placed");
                    }
                }

                if (cartsDto.Any())
                {
                    var not_in_cart = (from x in ordersDto
                                       where !(cartsDto.Any(y => y.ProductId.Equals(x.ProductId)))
                                       select x).Select(x => x.ProductId);
                    foreach (string productId in not_in_cart)
                    {
                        errors.Add($"ProductId {productId} not in cart");
                    }

                    var diff_quantity_cart_order = (from x in ordersDto
                                                    where (cartsDto.All(y => y.Qty != x.Qty))
                                                    select x).Select(x => x.ProductId);
                    foreach (string productId in not_in_cart)
                    {
                        errors.Add($"ProductId {productId} has different quantities in order and in cart.");
                    }
                }
                else
                {
                    foreach (OrderLineDto orderDto in ordersDto)
                    {
                        errors.Add($"ProductId {orderDto.ProductId} not in cart");
                    }
                }

                if (productsDto.Any())
                {
                    var order_exceed_stock = (from x in ordersDto
                                     join y in productsDto on x.ProductId equals y.Id
                                     where x.Qty > y.MaxQty
                                     select x).Select(x => x.ProductId);
                    foreach (string productId in order_exceed_stock)
                    {
                        errors.Add($"ProductId {productId} quantity is greather than quantity in stock");
                    }

                    var not_in_catalogue = (from x in ordersDto
                                       where !(productsDto.Any(y => y.Id.Equals(x.ProductId)))
                                       select x).Select(x => x.ProductId);
                    foreach (string productId in not_in_catalogue)
                    {
                        errors.Add($"ProductId {productId} not in catalogue");
                    }
                }
                else
                {
                    foreach (OrderLineDto orderDto in ordersDto)
                    {
                        errors.Add($"ProductId {orderDto.ProductId} not registered in stock");
                    }
                }
                return errors;
            });
        }
    }
}
