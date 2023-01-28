using AutoMapper;
using maplr_api.Context;
using maplr_api.DTO;
using maplr_api.Interfaces;
using maplr_api.Mappers;
using maplr_api.Models;
using maplr_api.Utils;
using System.Linq.Expressions;

namespace maplr_api.Repository
{
    public class ProductRepository : IProductRepository
    {
        private readonly MaplrContext _context;
        private readonly IMapper _mapper;

        public ProductRepository(MaplrContext maplrContext)
        {
            _context = maplrContext;
            _mapper = MapFields.InitializeRepositoryAutomapper();
        }

        public Task<IQueryable<CatalogueItemDto>> Get(Enums.Type type)
        {
            return Task.Run(() =>
            {
                var dbSet = _context.MapleSyrup;
                IQueryable<MapleSyrup> data;
                if (type <= 0)
                {
                    data = dbSet.AsQueryable();
                }
                else
                {
                    data = dbSet.Where(x => x.Type.Equals(type)).AsQueryable();
                }

                if (data.Any())
                {
                    var responseList = new List<CatalogueItemDto>();
                    foreach (MapleSyrup mapleSyrup in data)
                    {
                        responseList.Add(_mapper.Map<CatalogueItemDto>(mapleSyrup));
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
                var mapleSyrup = _context.MapleSyrup.FirstOrDefault(x => x.Id.Equals(key));
                if(mapleSyrup != null)
                {
                    return _mapper.Map<MapleSyrupDto>(mapleSyrup);
                }
                return null;
            });
        }
    }
}
