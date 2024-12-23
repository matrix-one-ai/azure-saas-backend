using Abp;
using System.Threading.Tasks;

namespace Icon.Authorization.Users.DataCleaners
{
    public interface IUserDataCleaner
    {
        Task CleanUserData(UserIdentifier userIdentifier);
    }
}
