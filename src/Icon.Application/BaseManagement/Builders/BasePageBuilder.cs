using System.Collections.Generic;
using Abp.UI;

namespace Icon.BaseManagement
{
    public class BasePageBuilder
    {
        public BasePageDto Build(BasePageType pageType, BaseListType listType)
        {
            switch (pageType)
            {
                case BasePageType.Memories:
                    return BuildMemoriesPage(pageType, listType);
                case BasePageType.CharacterPersonas:
                    return BuildCharacterPersonasPage(pageType, listType);
                case BasePageType.Characters:
                    return BuildCharactersPage(pageType, listType);
                default:
                    throw new UserFriendlyException($"BasePageBuilder - Unknown page type: {pageType}");
            }
        }

        public BasePageDto BuildMemoriesPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Memory,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildCharacterPersonasPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Persona,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildCharactersPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Character,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }
    }
}
