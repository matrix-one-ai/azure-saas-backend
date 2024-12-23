using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Icon.EntityChanges.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.EntityChanges
{
    public interface IEntityChangeAppService : IApplicationService
    {
        Task<ListResultDto<EntityAndPropertyChangeListDto>> GetEntityChangesByEntity(GetEntityChangesByEntityInput input);
    }
}
