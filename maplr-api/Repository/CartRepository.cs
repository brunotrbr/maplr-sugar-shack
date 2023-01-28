using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;
using Microsoft.EntityFrameworkCore;
using maplr_api.Mappers;

namespace maplr_api.Repository
{
    public class CartRepository : ICartRepository
    {
        private readonly MaplrContext _context;
        private readonly IMapper _mapper;

        public CartRepository(MaplrContext maplrContext, IMapper mapper)
        {
            _context = maplrContext;
            _mapper = MapFields.InitializeRepositoryAutomapper();
        }

        public Task<IQueryable<CartLineDto>> Get(string productId = "")
        {
            return Task.Run(() =>
            {
                throw new Exception("Erro interno do servidor");
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
                        responseList.Add(_mapper.Map<CartLineDto>(carts));
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
                    return _mapper.Map<CartLineDto>(carts);
                }
                return null;
            });
        }

        public Task<CartLineDto> Update(CartLineDto entity)
        {
            return Task.Run(() =>
            {
                var cart = _mapper.Map<Carts>(entity);
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
                var cart = _mapper.Map<Carts>(entity);
                _context.Update(cart);
                _context.SaveChanges();
                return entity;
            });
        }
    }
}
