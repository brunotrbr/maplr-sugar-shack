using maplr_api.Models;

namespace maplr_api.Interfaces
{
    public interface IMapleSyrupRepository
    {
        Task<IQueryable<MapleSyrup>> Get();

        Task<MapleSyrup?> GetByKey(string key);

        Task<MapleSyrup> Insert(MapleSyrup entity);

        Task<MapleSyrup> Update(string key, MapleSyrup entity);

        Task<string> Delete(string key);
    }
}
