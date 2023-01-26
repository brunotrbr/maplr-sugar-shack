using maplr_api.Context;
using maplr_api.Interfaces;
using maplr_api.Models;

namespace maplr_api.Repository
{
    public class MapleSyrupRepository : IMapleSyrupRepository
    {
        private readonly MaplrContext _context;

        public MapleSyrupRepository(MaplrContext maplrContext)
        {
            _context = maplrContext;
        }

        public Task<string> Delete(string key)
        {
            return Task.Run(() =>
            {
                var entity = _context.Find<string>(key);

                if (entity == null)
                {
                    throw new Exception("ID inexistente.");
                }

                _context.Remove(entity);
                _context.SaveChanges();
                return key;
            });
        }

        public Task<IQueryable<MapleSyrup>> Get()
        {
            return Task.Run(() =>
            {
                var data = _context.Set<MapleSyrup>().AsQueryable();
                return data.Any() ? data : new List<MapleSyrup>().AsQueryable();
            });
        }

        public Task<MapleSyrup?> GetByKey(string key)
        {
            return Task.Run(() =>
            {
                return _context.Find<MapleSyrup>(key);
            });
        }

        public Task<MapleSyrup> Insert(MapleSyrup entity)
        {
            return Task.Run(() =>
            {
                _context.Add(entity);
                _context.SaveChanges();
                return entity;
            });
        }

        public Task<MapleSyrup> Update(string key, MapleSyrup entity)
        {
            return Task.Run(() =>
            {
                _context.Update(entity);
                _context.SaveChanges();
                return entity;
            });
        }
    }
}
