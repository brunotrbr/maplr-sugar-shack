using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;
using maplr_api.Utils;
using System.Linq.Expressions;

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

        static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<MapleSyrup, CatalogueItemDto>()
                  .ForSourceMember(src => src.Description, opt => opt.DoNotValidate())
                   .AfterMap((src, dest) => dest.MaxQty = src.Stock);

                cfg.CreateMap<MapleSyrup, MapleSyrupDto>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        private CartLineDto mapleSyrupToCartLineDto(MapleSyrup mapleSyrup)
        {
            return _mapper.Map<CartLineDto>(mapleSyrup);
        }

        //private MapleSyrupDto mapleSyrupToDto(MapleSyrup mapleSyrup)
        //{
        //    return _mapper.Map<MapleSyrupDto>(mapleSyrup);
        //}

        public Task<IQueryable<CartLineDto>> Get()
        {
            return Task.Run(() =>
            {
                var query = _context.Set<MapleSyrup>().AsQueryable();

                if (query.Any())
                {
                    var responseList = new List<CartLineDto>();
                    foreach (MapleSyrup mapleSyrup in query)
                    {
                        responseList.Add(mapleSyrupToCartLineDto(mapleSyrup));
                    }
                    return responseList.AsQueryable();
                }
                return new List<CartLineDto>().AsQueryable();
            });
        }

        public Task<CartLineDto> Update(string key, CartLineDto entity)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> Delete(string key)
        {
            throw new NotImplementedException();
        }

        public Task<CartLineDto> Patch(string key, CartLineDto entity)
        {
            throw new NotImplementedException();
        }
    }
}
