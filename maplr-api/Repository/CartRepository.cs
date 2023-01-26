using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;
using Microsoft.EntityFrameworkCore;

namespace maplr_api.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly MaplrContext _context;
        public readonly IMapper _mapper;

        public CartRepository(MaplrContext maplrContext)
        {
            _context = maplrContext;
            _mapper = InitializeAutomapper();
        }

        private static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Carts, CartLineDto>();

                cfg.CreateMap<CartLineDto, Carts>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        private CartLineDto CartToCartLineDto(Carts carts)
        {
            return _mapper.Map<CartLineDto>(carts);
        }

        private Carts CartLineDtoToCarts(CartLineDto cartLineDto)
        {
            return _mapper.Map<Carts>(cartLineDto);
        }

        public Task<IQueryable<CartLineDto>> Get(string productId = "")
        {
            return Task.Run(() =>
            {
                var dbset = _context.Carts;
                IQueryable<Carts> data;

                if (string.IsNullOrWhiteSpace(productId))
                {
                    data = dbset.AsQueryable();
                }
                else
                {
                    data = dbset.Where(x => x.ProductId.Equals(productId));
                }

                if (data.Any())
                {
                    var responseList = new List<CartLineDto>();
                    foreach (Carts carts in data)
                    {
                        responseList.Add(CartToCartLineDto(carts));
                    }
                    return responseList.AsQueryable();
                }
                return new List<CartLineDto>().AsQueryable();
            });
        }

        public Task<CartLineDto?> GetByKey(string key)
        {
            return Task.Run(() =>
            {
                var carts = _context.Carts.AsNoTracking().FirstOrDefault(x => x.ProductId.Equals(key));
                if (carts != null)
                {
                    return CartToCartLineDto(carts);
                }
                return null;
            });
        }

        public Task<CartLineDto> Update(CartLineDto entity)
        {
            return Task.Run(() =>
            {
                var cart = CartLineDtoToCarts(entity);
                _context.Add(cart);
                _context.SaveChanges();
                return entity;
            });
        }

        public Task<string> Delete(string key)
        {
            return Task.Run(() =>
            {
                var entity = _context.Carts.First(x => x.ProductId.Equals(key));
                _context.Remove(entity);
                _context.SaveChanges();
                return key;
            });
        }

        public Task<CartLineDto> Patch(CartLineDto entity)
        {
            return Task.Run(() =>
            {
                var cart = CartLineDtoToCarts(entity);
                _context.Update(cart);
                _context.SaveChanges();
                return entity;
            });
        }
    }
}
