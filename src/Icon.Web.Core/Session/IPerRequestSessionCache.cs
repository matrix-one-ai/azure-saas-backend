using System.Threading.Tasks;
using Icon.Sessions.Dto;

namespace Icon.Web.Session
{
    public interface IPerRequestSessionCache
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformationsAsync();
    }
}
