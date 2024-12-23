using System.Collections.Generic;
using System.Threading.Tasks;
using Abp;
using Icon.Dto;

namespace Icon.Gdpr
{
    public interface IUserCollectedDataProvider
    {
        Task<List<FileDto>> GetFiles(UserIdentifier user);
    }
}
