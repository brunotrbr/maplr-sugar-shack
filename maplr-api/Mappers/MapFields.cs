using AutoMapper;
using maplr_api.DTO;
using maplr_api.Models;

namespace maplr_api.Mappers
{
    public class MapFields
    {
        public static Mapper InitializeRepositoryAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region Map CartRepository
                cfg.CreateMap<Carts, CartLineDto>();

                cfg.CreateMap<CartLineDto, Carts>();
                #endregion

                #region Map OrderRepository
                cfg.CreateMap<Orders, OrderLineDto>();

                cfg.CreateMap<OrderLineDto, Orders>();
                #endregion

                # region Map ProductRepository
                cfg.CreateMap<MapleSyrup, CatalogueItemDto>()
                  .ForSourceMember(src => src.Description, opt => opt.DoNotValidate())
                   .AfterMap((src, dest) => dest.MaxQty = src.Stock);

                cfg.CreateMap<MapleSyrup, MapleSyrupDto>();
                #endregion

                #region Map CartController
                cfg.CreateMap<MapleSyrupDto, CartLineDto>()
                  .ForSourceMember(src => src.Description, opt => opt.DoNotValidate())
                  .ForSourceMember(src => src.Type, opt => opt.DoNotValidate())
                  .AfterMap((src, dest) => { dest.ProductId = src.Id; dest.Qty = src.Stock > 0 ? 1 : 0; });
                #endregion
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        public static Mapper InitializeControllerAutomapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                #region Map CartController
                cfg.CreateMap<MapleSyrupDto, CartLineDto>()
                  .ForSourceMember(src => src.Description, opt => opt.DoNotValidate())
                  .ForSourceMember(src => src.Type, opt => opt.DoNotValidate())
                  .AfterMap((src, dest) => { dest.ProductId = src.Id; dest.Qty = src.Stock > 0 ? 1 : 0; });
                #endregion
            });
            var mapper = new Mapper(config);
            return mapper;
        }
    }
}
