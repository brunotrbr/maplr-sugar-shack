using maplr_api.DTO;
using maplr_api.Interfaces;

namespace maplr_api.BusinessLayers
{
    public class OrdersBL
    {
        private readonly IProductRepository _productRepository;
        private readonly ICartRepository _cartRepository;

        public OrdersBL(IProductRepository productRepository, ICartRepository cartRepository)
        {
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

                var cartsDto = _cartRepository.Get().Result;
                var productsDto = _productRepository.Get(0).Result;

                if (cartsDto.Any())
                {
                    var not_in_cart = (from x in ordersDto
                                       where !(cartsDto.Any(y => y.ProductId.Equals(x.ProductId)))
                                       select x).Select(x => x.ProductId);
                    foreach (string productId in not_in_cart)
                    {
                        errors.Add($"ProductId {productId} not in cart");
                    }

                    foreach (var order in ordersDto)
                    {
                        foreach (var cart in cartsDto)
                        {
                            if (order.ProductId.Equals(cart.ProductId) && cart.Qty > 0 && order.Qty != cart.Qty)
                            {
                                errors.Add($"ProductId {order.ProductId} has different quantities in order and in cart");
                            }
                        }
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
                        errors.Add($"ProductId {orderDto.ProductId} not in catalogue");
                    }
                }
                return errors;
            });
        }
    }
}
