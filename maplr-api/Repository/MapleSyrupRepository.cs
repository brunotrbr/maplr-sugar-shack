using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Models;
using maplr_api.Utils;
using System.Linq.Expressions;

namespace maplr_api.Repository
{
    public class MapleSyrupRepository : IMapleSyrupRepository
    {
        private readonly MaplrContext _context;
        public readonly IMapper _mapper;

        public MapleSyrupRepository(MaplrContext maplrContext)
        {
            _context = maplrContext;
            _mapper = InitializeAutomapper();
        }

        static Mapper InitializeAutomapper()
        {
            var config = new MapperConfiguration(cfg => {
                cfg.CreateMap<MapleSyrup, CatalogueItemDto>()
                  .ForSourceMember(src => src.Description, opt => opt.DoNotValidate())
                   .AfterMap((src, dest) => dest.MaxQty = src.Stock);

                cfg.CreateMap<MapleSyrup, MapleSyrupDto>();
            });
            var mapper = new Mapper(config);
            return mapper;
        }

        private CatalogueItemDto mapleSyrupToCatalogueDto(MapleSyrup mapleSyrup)
        {
            return _mapper.Map<CatalogueItemDto>(mapleSyrup);
        }

        private MapleSyrupDto mapleSyrupToDto(MapleSyrup mapleSyrup)
        {
            return _mapper.Map<MapleSyrupDto>(mapleSyrup);
        }

        public Task<IQueryable<CatalogueItemDto>> Get(Enums.Type type)
        {
            return Task.Run(() =>
            {
                var query = _context.Set<MapleSyrup>().AsQueryable();

                if (type != 0)
                {
                    query = query.Where(x => x.Type.Equals(type));
                }

                if (query.Any())
                {
                    var responseList = new List<CatalogueItemDto>();
                    foreach (MapleSyrup mapleSyrup in query)
                    {
                        responseList.Add(mapleSyrupToCatalogueDto(mapleSyrup));
                    }
                    return responseList.AsQueryable();
                }
                return new List<CatalogueItemDto>().AsQueryable();
            });
        }

        public Task<MapleSyrupDto?> GetByKey(string key)
        {
            return Task.Run(() =>
            {

                var mapleSyrup = _context.Find<MapleSyrup>(key);
                if(mapleSyrup != null)
                {
                    return mapleSyrupToDto(mapleSyrup);
                }
                return null;
            });
        }
    }
}
