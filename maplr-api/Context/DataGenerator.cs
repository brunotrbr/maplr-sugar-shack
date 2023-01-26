using maplr_api.Models;
using System.Text.Json;

namespace maplr_api.Context
{
    public class DataGenerator
    {
        private readonly MaplrContext _maplrContext;

        public DataGenerator(MaplrContext maplrContext)
        {
            _maplrContext = maplrContext;
        }

        public void InsertData()
        {
            if (_maplrContext.MapleSyrup.Any() == false)
            {
                List<MapleSyrup> items = new List<MapleSyrup>();
                using (StreamReader r = new StreamReader("MapleSyrup.json"))
                {
                    string json = r.ReadToEnd();
                    items = JsonSerializer.Deserialize<List<MapleSyrup>>(json);
                }

                _maplrContext.MapleSyrup.AddRange(entities: items);
                _maplrContext.SaveChanges();
            }
        }
    }
}
