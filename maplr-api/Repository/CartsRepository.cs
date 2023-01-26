using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;

namespace maplr_api.Repository
{
    public class CartsRepository : ICartsRepository
    {
        private readonly MaplrContext _context;
        public readonly IMapper _mapper;

        public CartsRepository(MaplrContext maplrContext)
        {
            _context = maplrContext;
            _mapper = InitializeAutomapper();
        }

        static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Carts, CartLineDto>()
                  .ForSourceMember(src => src.Id, opt => opt.DoNotValidate());
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        private CartLineDto CartsToCartLineDto(Carts carts)
        {
            return _mapper.Map<CartLineDto>(carts);
        }

        public Task<IQueryable<CartLineDto>> Get()
        {
            return Task.Run(() =>
            {
                var query = _context.Set<Carts>().AsQueryable();

                if (query.Any())
                {
                    var responseList = new List<CartLineDto>();
                    foreach (Carts carts in query)
                    {
                        responseList.Add(CartsToCartLineDto(carts));
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

                var carts = _context.Find<Carts>(key);
                if(carts != null)
                {
                    return CartsToCartLineDto(carts);
                }
                return null;
            });
        }

        public Task<CartLineDto> Update(string key, CartLineDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<string> Delete(string key)
        {
            return Task.Run(() =>
            {
                var entity = _context.Find<Carts>(key);

                if (entity == null)
                {
                    throw new Exception("ID inexistente.");
                }

                _context.Remove(entity);
                _context.SaveChanges();
                return key;
            });
        }

        public Task<CartLineDto> Patch(string key, CartLineDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
