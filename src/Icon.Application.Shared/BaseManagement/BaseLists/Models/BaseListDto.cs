using System;
using System.Collections.Generic;

namespace Icon.BaseManagement
{
    public class BaseListDto
    {
        public string ListTitle { get; set; }
        public string EntityName { get; set; }
        public string ListIcon { get; set; }
        public string HeaderClass { get; set; }
        public List<BaseListColumnDto> Columns { get; set; }
        public List<BaseListRowActionDto> RowActions { get; set; }
        public List<BaseListHeaderActionDto> HeaderActions { get; set; }
        public BaseListSourceDto Source { get; set; }
        public List<BaseListFilterDto> Filters { get; set; }
        public List<BaseListFilterButtonDto> FilterButtons { get; set; }
        public bool FiltersEnabled { get; set; }
        public bool FiltersShown { get; set; }
        public bool PageHeaderShow { get; set; }
        public string ActionsName { get; set; }
        public bool RowActionsEnabled { get; set; }
        public bool HeaderActionsEnabled { get; set; }
        public BaseListEvent RefreshEvent { get; set; }
        public BaseListDto()
        {
            Columns = new List<BaseListColumnDto>();
            RowActions = new List<BaseListRowActionDto>();
            HeaderActions = new List<BaseListHeaderActionDto>();
            Filters = new List<BaseListFilterDto>();
            FilterButtons = new List<BaseListFilterButtonDto>();
        }

        public void SetFilterOptions(string filterLabel, List<BaseListDropdownOptionDto> options)
        {
            var filter = Filters.Find(f => f.Label == filterLabel);
            filter.FilterOptions = options;
        }

        public void ApplyListSettings(BaseListSettingsDto settings)
        {
            if (settings == null)
            {
                return;
            }

            FiltersEnabled = settings.FiltersEnabled;
            FiltersShown = settings.FiltersEnabled;
            PageHeaderShow = settings.ShowPageHeader;
            HeaderActionsEnabled = settings.HeaderActionsEnabled;

            if (settings.FilterFromDate.HasValue)
            {
                var filterFromDate = Filters.Find(f => f.FilterType == BaseListFilterType.FromDate);
                var filterFromTime = Filters.Find(f => f.FilterType == BaseListFilterType.FromTime);
                var filterRange = Filters.Find(f => f.FilterType == BaseListFilterType.DateRange);
                if (filterFromDate != null)
                {
                    filterFromDate.DateFromValue = settings.FilterFromDate;
                    filterFromDate.MinDate = settings.FilterFromDateMin;
                    filterFromDate.MaxDate = settings.FilterFromDateMax;
                }
                if (filterFromTime != null)
                {
                    filterFromTime.DateFromValue = settings.FilterFromDate;
                }
                if (filterRange != null)
                {
                    filterRange.DateFromValue = settings.FilterFromDate;
                    filterRange.MinDate = settings.FilterFromDateMin;
                    filterRange.MaxDate = settings.FilterFromDateMax;
                }
            }

            if (settings.FilterToDate.HasValue)
            {
                var filterToDate = Filters.Find(f => f.FilterType == BaseListFilterType.ToDate);
                var filterToTime = Filters.Find(f => f.FilterType == BaseListFilterType.ToTime);
                var filterRange = Filters.Find(f => f.FilterType == BaseListFilterType.DateRange);
                if (filterToDate != null)
                {
                    filterToDate.DateToValue = settings.FilterToDate;
                    filterToDate.MinDate = settings.FilterToDateMin;
                    filterToDate.MaxDate = settings.FilterToDateMax;
                }
                if (filterToTime != null)
                {
                    filterToTime.DateToValue = settings.FilterToDate;
                };
                if (filterRange != null)
                {
                    filterRange.DateToValue = settings.FilterToDate;
                    filterRange.MinDate = settings.FilterToDateMin;
                    filterRange.MaxDate = settings.FilterToDateMax;
                }
            }

            RemoveHeaderActions(settings.RemoveHeaderActions);
            RemoveRowActions(settings.RemoveRowActions);
            RemoveColumns(settings.RemoveColumns);
            RemoveFilters(settings.RemoveFilters);
        }

        private void RemoveHeaderActions(List<BaseListHeaderActionType> headerActions)
        {
            if (headerActions == null || headerActions.Count == 0)
            {
                return;
            }
            HeaderActions.RemoveAll(h => headerActions.Contains(h.ActionType));
        }
        private void RemoveRowActions(List<BaseListRowActionType> rowActionType)
        {
            if (rowActionType == null || rowActionType.Count == 0)
            {
                return;
            }
            RowActions.RemoveAll(r => rowActionType.Contains(r.ActionType));
        }

        private void RemoveColumns(List<string> columnNames)
        {
            if (columnNames == null || columnNames.Count == 0)
            {
                return;
            }
            Columns.RemoveAll(c => columnNames.Contains(c.Name));
        }
        private void RemoveFilters(List<string> filterNames)
        {
            if (filterNames == null || filterNames.Count == 0)
            {
                return;
            }
            Filters.RemoveAll(f => filterNames.Contains(f.Name));
        }


    }

    public class BaseListSettingsDto
    {
        public bool ShowPageHeader { get; set; }
        public bool FiltersEnabled { get; set; }
        public bool RowActionsEnabled { get; set; }
        public bool HeaderActionsEnabled { get; set; }
        public DateTime? FilterFromDate { get; set; }
        public DateTime? FilterFromDateMin { get; set; }
        public DateTime? FilterFromDateMax { get; set; }
        public DateTime? FilterToDate { get; set; }
        public DateTime? FilterToDateMin { get; set; }
        public DateTime? FilterToDateMax { get; set; }
        public List<string> RemoveFilters { get; set; }
        public List<string> RemoveColumns { get; set; }
        public List<BaseListRowActionType> RemoveRowActions { get; set; }
        public List<BaseListHeaderActionType> RemoveHeaderActions { get; set; }
    }

    public class BaseListSourceDto
    {
        public string FrontEndComponent { get; set; }
        public string BackendServiceName { get; set; }
        public string BackendServiceMethod { get; set; }
        public string BackendFilterInput { get; set; }
    }

    public class BaseListColumnDto
    {
        public string Name { get; set; }
        public string Header { get; set; }
        public string SortPath { get; set; }
        public string ValuePath { get; set; }
        public int Width { get; set; }
        public bool CanSort { get; set; }
        public bool ShouldTranslate { get; set; }
        public bool ShouldTruncate { get; set; }
        public bool UseTemplate { get; set; }
        public BaseListTemplateType Template { get; set; }
        public BaseListNavigationDto Navigation { get; set; }
        public bool ShouldPipe { get; set; }
        public BaseListPipeType PipeType { get; set; }
        public string PipeFormat { get; set; }
    }

    public class BaseListNavigationDto
    {
        public string Icon { get; set; }
        public string ValuePath { get; set; }
        public string ValueSubPath { get; set; }
        public string Event { get; set; }
        public string Route { get; set; }
    }


    public enum BaseListPipeType
    {
        Date,
        Number,
        Currency
    }

    public enum BaseListTemplateType
    {
        None,
        Status,
        Navigation,
        Url,
        ArrivalDeparture,
        RowActions,
        BlockadeRowActions,
        Ranking
    }

    public enum BaseListEvent
    {
        RefreshList,
    }

    public enum BaseListType
    {
        MemoriesList,
        CharacterList,
        CharacterPersonaList,
    }

    public class BaseListHeaderActionDto
    {
        // dont use subclasses for this dto
        public string Name { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string SubIcon { get; set; }
        public string Class { get; set; }
        public string Tooltip { get; set; }
        public string Event { get; set; }
        public BaseListHeaderActionType ActionType { get; set; }
        public bool OpenModal { get; set; }
        public BaseModalType ModalType { get; set; }
        public Guid? ClientId { get; set; }
        public Guid? LocationId { get; set; }
        public string BackendServiceName { get; set; }
        public string BackendMethodName { get; set; }
        public string BackendInputName { get; set; }

        public BaseListHeaderActionDto(
            BaseListHeaderActionType actionType,
            BaseModalType modalType,
            BaseBackendEventDto backendEvent)
        {
            ActionType = actionType;
            Label = "List.HeaderAction." + actionType.ToString();
            Tooltip = "List.ToolTrip." + actionType.ToString();
            OpenModal = true;
            ModalType = modalType;

            BackendServiceName = backendEvent.BackendServiceName;
            BackendMethodName = backendEvent.BackendMethodName;
            BackendInputName = backendEvent.BackendInputName;

            SetActionSettings();
        }

        private void SetActionSettings()
        {
            if (OpenModal)
            {
                Event = "app.baseModal.listHeaderAction";
            }

            if (ActionType == BaseListHeaderActionType.NewPersona)
            {
                Icon = BaseIconOptions.Create;
                Class = "btn btn-sm btn-outline-info btn-light-info";
                SubIcon = BaseIconOptions.Persona;
            }

            if (ActionType == BaseListHeaderActionType.NewCharacter)
            {
                Icon = BaseIconOptions.Create;
                Class = "btn btn-sm btn-outline-info btn-light-info";
                SubIcon = BaseIconOptions.Character;
            }
            // else if (ActionType == BaseListHeaderActionType.NewClientBlockade || ActionType == BaseListHeaderActionType.NewClientBlockadeWithClient)
            // {
            //     Icon = BaseIconOptions.Create;
            //     SubIcon = BaseIconOptions.Blockade;
            //     Class = "btn btn-sm btn-outline-info btn-light-info";
            // }
            // else if (ActionType == BaseListHeaderActionType.NewGeneralChange)
            // {
            //     Icon = BaseIconOptions.Create;
            //     SubIcon = BaseIconOptions.ChangeRequest;
            //     Class = "btn btn-sm btn-outline-info btn-light-info";
            // }
        }
    }

    public enum BaseListHeaderActionType
    {
        NewPersona,
        NewCharacter
    }


}