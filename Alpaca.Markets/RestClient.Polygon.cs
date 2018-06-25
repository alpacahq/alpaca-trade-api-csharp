using System.Collections.Generic;
using System.Threading.Tasks;

namespace Alpaca.Markets
{
    public sealed partial class RestClient
    {
        public Task<IEnumerable<IExchange>> GetExchangesAsync()
        {
            return getObjectsListAsync<IExchange, JsonExchange>(
                _polygonHttpClient, "/v1/meta/exchanges");
        }
    }
}