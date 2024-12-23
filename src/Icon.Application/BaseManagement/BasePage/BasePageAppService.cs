using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Abp.Authorization;
using Icon.BaseManagement.BasePage.Requests;
using System.Linq;
using System.Collections.Generic;
using Icon.Matrix;

namespace Icon.BaseManagement
{
    [AbpAuthorize]
    public class BasePageAppService : IconAppServiceBase
    {
        private readonly IPermissionChecker _permissionChecker;
        private readonly IMemoryManager _memoryManager;

        public BasePageAppService(
            IPermissionChecker permissionChecker,
            IMemoryManager memoryManager)
        {
            _permissionChecker = permissionChecker;
            _memoryManager = memoryManager;
        }

        [HttpPost]
        public async Task<BasePageDto> GetBasePage(GetBasePageInput input)
        {
            await Task.CompletedTask;
            var page = new BasePageBuilder().Build(input.PageType, input.ListType);

            if (page.List != null)
            {
                foreach (var filter in page.List.Filters)
                {
                    if (filter.FilterType == BaseListFilterType.SingleSelect)
                    {
                        foreach (var option in filter.FilterOptions)
                        {
                            option.Name = L(option.Name);
                        }

                        filter.FilterOptions = filter.FilterOptions.OrderBy(x => x.Name).ToList();
                        filter.FilterOptions.Insert(0, new BaseListDropdownOptionDto
                        {
                            Id = null,
                            Name = L("All")
                        });

                        if (filter.FilterPath == "memoryTypeId")
                        {
                            await AddDropDownMemoryTypes(filter.FilterOptions);
                        }
                    }
                }
            }

            return page;
        }

        private async Task AddDropDownMemoryTypes(List<BaseListDropdownOptionDto> options)
        {
            var memoryTypes = await _memoryManager.GetMemoryTypes();
            foreach (var memoryType in memoryTypes)
            {
                options.Add(new BaseListDropdownOptionDto
                {
                    Id = memoryType.Id,
                    Name = L(memoryType.Name)
                });
            }
        }
    }
}