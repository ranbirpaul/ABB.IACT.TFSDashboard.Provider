using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;

namespace ABB.IACT.TFSDashboard.Provider.Services
{
    public interface ISearchServices
    {
        Task<List<JObject>> ExecuteQuery(string query);
    }
}
