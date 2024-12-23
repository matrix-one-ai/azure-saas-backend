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

        public BasePageDto BuildLocationsPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Location,
                PageHeaderShow = true,
                List = new BaseListBuilder().Build(listType)
            };

            return page;
        }

        public BasePageDto BuildLocationTripsPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Trip,
                List = new BaseListBuilder().Build(listType),
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildLocationRoutesPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Route,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildLocationClientPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Client,
                PageHeaderShow = false
            };

            return page;
        }

        public BasePageDto BuildLocationClientsPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Client,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildLocationBlockadesPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Blockade,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildLocationChangeRequestsPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.ChangeRequest,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

        public BasePageDto BuildLocationSchedulesPage(BasePageType pageType, BaseListType listType)
        {
            var page = new BasePageDto
            {
                PageTitle = "Page.Title." + pageType,
                PageIcon = BaseIconOptions.Schedule,
                List = new BaseListBuilder().Build(listType)
            };
            page.PageHeaderShow = page.List.PageHeaderShow;

            return page;
        }

    }
}
