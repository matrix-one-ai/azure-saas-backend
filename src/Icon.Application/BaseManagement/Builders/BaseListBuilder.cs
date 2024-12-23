using System;
using System.Collections.Generic;
using Abp.UI;
using Icon.Matrix;
using Icon.Matrix.CharacterPersonas;
using Icon.Matrix.Characters;
using Icon.Matrix.Memories;

namespace Icon.BaseManagement
{
    public class BaseListBuilder
    {
        public BaseListDto Build(BaseListType listType)
        {
            switch (listType)
            {
                case BaseListType.MemoriesList:
                    return BuildMemoriesList(listType);
                case BaseListType.CharacterList:
                    return BuildCharacterList(listType);
                case BaseListType.CharacterPersonaList:
                    return BuildCharacterPersonaList(listType);

                default:
                    throw new UserFriendlyException($"BaseListBuilder - Unknown list type: {listType}");
            }
        }

        private BaseListDto BuildMemoriesList(BaseListType listType)
        {
            var settings = new BaseListSettingsDto
            {
                ShowPageHeader = true,
                FiltersEnabled = true,
                RowActionsEnabled = true,

                FilterFromDate = DateTime.Now,
                FilterFromDateMin = DateTime.Now.AddDays(-14),
                FilterFromDateMax = DateTime.Now.AddDays(90),

                FilterToDate = DateTime.Now.AddDays(14),
                FilterToDateMin = DateTime.Now.AddDays(-14),
                FilterToDateMax = DateTime.Now.AddDays(90),
            };

            var list = new BaseListDto
            {
                ListTitle = SetListTitle(listType),
                ListIcon = BaseIconOptions.Memory,
                EntityName = "Memory",
                HeaderClass = "super-header",
                PageHeaderShow = true,
                Source = new BaseListSourceDto
                {
                    FrontEndComponent = "memories.component",
                    BackendServiceName = BaseHelper.GetServiceName(nameof(MemoryAppService)),
                    BackendServiceMethod = BaseHelper.GetMethodName(nameof(MemoryAppService.GetMemories)),
                    BackendFilterInput = nameof(GetMemoriesInput),
                },
                RowActionsEnabled = true,
                FiltersEnabled = true,
                FiltersShown = true,
                Filters = new List<BaseListFilterDto>
                {
                    BaseListFilterFactory.GetSingleSelectFilter(label: "MemoryType", filterPath: "memoryTypeId"),
                    BaseListFilterFactory.GetMemoryCharacterFilter(),
                    BaseListFilterFactory.GetMemoryPersonaFilter(),
                    BaseListFilterFactory.GetMemoryContentFilter(),
                    BaseListFilterFactory.GetDateRangeFilter(),
                },
                FilterButtons = new List<BaseListFilterButtonDto>
                {
                    FilterButtonOptionsDto.Reset,
                    FilterButtonOptionsDto.Search
                },
                Columns = new List<BaseListColumnDto>
                {
                    BaseListColumnFactory.GetDateColumn(valuePath: "platformInteractionDate", header: "Date"),
                    BaseListColumnFactory.GetTimeColumn(valuePath: "platformInteractionDate", header: "Time"),
                    BaseListColumnFactory.GetPlatformColumn(),
                    BaseListColumnFactory.GetCharacterColumn(),
                    BaseListColumnFactory.GetPersonaColumn(),
                    //BaseListColumnFactory.GetTwitterRankColumn(),
                    BaseListColumnFactory.GetMemoryTypeColumn(),
                    BaseListColumnFactory.GetMemoryContentColumn(),
                    BaseListColumnFactory.GetMemoryUrlColumn(),
                    BaseListColumnFactory.GetPromptStatusColumn(),
                    BaseListColumnFactory.GetActionStatusColumn(),
                    BaseListColumnFactory.GetRowActionsColumn(entityName: "Memory"),
                },
                RowActions = new List<BaseListRowActionDto>
                {
                    new BaseListRowActionDto(
                        actionType: BaseListRowActionType.Open,
                        entityIdPath: "id",
                        tooltip: "List.Tooltip.MemoryView",
                        modalType: BaseModalType.ModalView,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(MemoryAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(MemoryAppService.GetModalView)),
                        }
                    ),
                    new BaseListRowActionDto(
                        actionType: BaseListRowActionType.Edit,
                        entityIdPath: "id",
                        tooltip: "List.Tooltip.MemoryEdit",
                        modalType: BaseModalType.ModalEdit,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(MemoryAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(MemoryAppService.GetModalEdit)),
                        }
                    ),
                }
            };

            list.ApplyListSettings(settings);

            return list;
        }

        private BaseListDto BuildCharacterList(BaseListType listType)
        {
            var settings = new BaseListSettingsDto
            {
                ShowPageHeader = true,
                FiltersEnabled = true,
                RowActionsEnabled = true,
                HeaderActionsEnabled = true,
            };

            var list = new BaseListDto
            {
                ListTitle = SetListTitle(listType),
                ListIcon = BaseIconOptions.Character,
                EntityName = "Character",
                HeaderClass = "super-header",
                PageHeaderShow = true,
                Source = new BaseListSourceDto
                {
                    FrontEndComponent = "characters.component",
                    BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterAppService)),
                    BackendServiceMethod = BaseHelper.GetMethodName(nameof(CharacterAppService.GetCharacters)),
                    BackendFilterInput = nameof(GetCharactersInput),
                },
                RowActionsEnabled = true,
                HeaderActionsEnabled = true,
                FiltersEnabled = true,
                FiltersShown = true,
                Filters = new List<BaseListFilterDto>
                {
                    BaseListFilterFactory.GetCharacterNameFilter(),
                },
                FilterButtons = new List<BaseListFilterButtonDto>
                {
                    FilterButtonOptionsDto.Reset,
                    FilterButtonOptionsDto.Search
                },
                Columns = new List<BaseListColumnDto>
                {
                    BaseListColumnFactory.GetCharacterNameColumn(),
                    BaseListColumnFactory.GetTwitterPostAgentIdColumn(),
                    BaseListColumnFactory.GetTwitterScrapeAgentIdColumn(),
                    BaseListColumnFactory.GetTwitterUserNameColumn(),
                    BaseListColumnFactory.GetIsTwitterScrapingEnabledColumn(),
                    BaseListColumnFactory.GetIsTwitterPostingEnabledColumn(),
                    BaseListColumnFactory.GetIsPromptingEnabledColumn(),
                    BaseListColumnFactory.GetRowActionsColumn(entityName: "Character"),
                },
                HeaderActions = new List<BaseListHeaderActionDto>
                {
                    new BaseListHeaderActionDto(
                        actionType: BaseListHeaderActionType.NewCharacter,
                        modalType: BaseModalType.ModalNew,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(CharacterAppService.GetModalNew)),
                        }
                    ),
                },
                RowActions = new List<BaseListRowActionDto>
                {
                    new BaseListRowActionDto(
                        actionType: BaseListRowActionType.Open,
                        entityIdPath: "id",
                        tooltip: "List.Tooltip.CharacterView",
                        modalType: BaseModalType.ModalView,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(CharacterAppService.GetModalView)),
                        }
                    ),
                    new BaseListRowActionDto(
                        actionType: BaseListRowActionType.Edit,
                        entityIdPath: "id",
                        tooltip: "List.Tooltip.CharacterEdit",
                        modalType: BaseModalType.ModalEdit,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(CharacterAppService.GetModalEdit)),
                        }
                    ),
                }
            };

            list.ApplyListSettings(settings);

            return list;

        }
        private BaseListDto BuildCharacterPersonaList(BaseListType listType)
        {
            var settings = new BaseListSettingsDto
            {
                ShowPageHeader = true,
                FiltersEnabled = true,
                RowActionsEnabled = true,
                HeaderActionsEnabled = true,
            };

            var list = new BaseListDto
            {
                ListTitle = SetListTitle(listType),
                ListIcon = BaseIconOptions.Persona,
                EntityName = "CharacterPersona",
                HeaderClass = "super-header",
                PageHeaderShow = true,
                Source = new BaseListSourceDto
                {
                    FrontEndComponent = "character-personas.component",
                    BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                    BackendServiceMethod = BaseHelper.GetMethodName(nameof(CharacterPersonaAppService.GetCharacterPersonas)),
                    BackendFilterInput = nameof(GetCharacterPersonasInput),
                },
                RowActionsEnabled = true,
                HeaderActionsEnabled = true,
                FiltersEnabled = true,
                FiltersShown = true,
                Filters = new List<BaseListFilterDto>
                {
                    BaseListFilterFactory.GetCharacterPersonaCharacterFilter(),
                    BaseListFilterFactory.GetCharacterPersonaFilter(),
                },
                FilterButtons = new List<BaseListFilterButtonDto>
                {
                    FilterButtonOptionsDto.Reset,
                    FilterButtonOptionsDto.Search
                },
                Columns = new List<BaseListColumnDto>
                {
                    BaseListColumnFactory.GetCharacterNameColumn(),
                    BaseListColumnFactory.GetPersonaNameColumn(),
                    BaseListColumnFactory.GetPlatformsColumn(valuePath: "persona.platformNames"),
                    BaseListColumnFactory.GetTwitterRankColumn(),
                    BaseListColumnFactory.GetPersonaAttitudeColumn(),
                    BaseListColumnFactory.GetPersonaShouldImportNewPostsColumn(),
                    BaseListColumnFactory.GetPersonaShouldRespondNewPostsColumn(),
                    BaseListColumnFactory.GetPersonaShouldRespondMentionsColumn(),
                    BaseListColumnFactory.GetRowActionsColumn(entityName: "CharacterPersona"),
                },
                HeaderActions = new List<BaseListHeaderActionDto>
                {
                    new BaseListHeaderActionDto(
                        actionType: BaseListHeaderActionType.NewPersona,
                        modalType: BaseModalType.ModalNew,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(CharacterPersonaAppService.GetModalNew)),
                        }
                    ),
                },
                RowActions = new List<BaseListRowActionDto>
                {
                    new BaseListRowActionDto(
                        actionType: BaseListRowActionType.Open,
                        entityIdPath: "id",
                        tooltip: "List.Tooltip.CharacterPersonaView",
                        modalType: BaseModalType.ModalView,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(CharacterPersonaAppService.GetModalView)),
                        }
                    ),
                    new BaseListRowActionDto(
                        actionType: BaseListRowActionType.Edit,
                        entityIdPath: "id",
                        tooltip: "List.Tooltip.CharacterPersonaEdit",
                        modalType: BaseModalType.ModalEdit,
                        backendEvent: new BaseBackendEventDto
                        {
                            BackendServiceName = BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                            BackendMethodName = BaseHelper.GetMethodName(nameof(CharacterPersonaAppService.GetModalEdit)),
                        }
                    ),
                }
            };

            list.ApplyListSettings(settings);

            return list;
        }


        private string SetListTitle(BaseListType listType)
        {
            return "List.Title." + listType.ToString();
        }
    }
}

