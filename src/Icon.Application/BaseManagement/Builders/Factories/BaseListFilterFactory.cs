using System;
using System.Collections.Generic;
using Azure.Core;


namespace Icon.BaseManagement
{
    public static class BaseListFilterFactory
    {
        public static BaseListFilterDto GetCharacterNameFilter()
        {
            return new BaseListFilterDto
            {
                Name = "CharacterNameFilter",
                Label = "CharacterNameFilter",
                PlaceHolder = "SearchCharacterName",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "characterName",
                EventOnChange = "fetchRecords",
            };
        }


        // GetCharacterPersonaCharacterFilter
        public static BaseListFilterDto GetCharacterPersonaCharacterFilter()
        {
            return new BaseListFilterDto
            {
                Name = "CharacterFilter",
                Label = "CharacterFilter",
                PlaceHolder = "SearchCharacter",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "characterName",
                EventOnChange = "fetchRecords",
            };
        }

        // GetCharacterPersonaFilter
        public static BaseListFilterDto GetCharacterPersonaFilter()
        {
            return new BaseListFilterDto
            {
                Name = "CharacterPersonaFilter",
                Label = "CharacterPersonaFilter",
                PlaceHolder = "SearchCharacterPersona",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "personaName",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetMemoryCharacterFilter()
        {
            return new BaseListFilterDto
            {
                Name = "MemoryCharacterFilter",
                Label = "MemoryCharacterFilter",
                PlaceHolder = "SearchMemoryCharacter",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "memoryCharacter",
                EventOnChange = "fetchRecords",
            };
        }

        // MemoryPersonaFilter
        public static BaseListFilterDto GetMemoryPersonaFilter()
        {
            return new BaseListFilterDto
            {
                Name = "MemoryPersonaFilter",
                Label = "MemoryPersonaFilter",
                PlaceHolder = "SearchMemoryPersona",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "memoryPersona",
                EventOnChange = "fetchRecords",
            };
        }
        public static BaseListFilterDto GetMemoryContentFilter()
        {
            return new BaseListFilterDto
            {
                Name = "MemoryContent",
                Label = "MemoryContentFilter",
                PlaceHolder = "SearchMemoryContent",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "memoryContent",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetExternalTripIdFilter()
        {
            return new BaseListFilterDto
            {
                Name = "ExternalTripIdFilter",
                Label = "ExternalTripIdFilter",
                PlaceHolder = "SearchExternalTripId",
                FilterType = BaseListFilterType.Number,
                Class = "filter-default-container",
                FilterPath = "externalId",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetExternalClientIdFilter()
        {
            return new BaseListFilterDto
            {
                Name = "ExternalClientIdFilter",
                Label = "ExternalClientIdFilter",
                PlaceHolder = "SearchExternalClientId",
                FilterType = BaseListFilterType.Number,
                Class = "filter-default-container",
                FilterPath = "externalId",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetExternalRouteIdFilter()
        {
            return new BaseListFilterDto
            {
                Name = "ExternalRouteIdFilter",
                Label = "ExternalRouteIdFilter",
                PlaceHolder = "SearchExternalRouteId",
                FilterType = BaseListFilterType.Number,
                Class = "filter-default-container",
                FilterPath = "externalId",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetClientNameFilter()
        {
            return new BaseListFilterDto
            {
                Name = "ClientNameFilter",
                Label = "ClientNameFilter",
                PlaceHolder = "SearchLastNameWithThreeDot",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "filterText",
                EventOnChange = "fetchRecords",
            };
        }



        public static BaseListFilterDto GetLocationNameFilter()
        {
            return new BaseListFilterDto
            {
                Name = "LocationNameFilter",
                Label = "LocationNameFilter",
                PlaceHolder = "SearchLocationName",
                FilterType = BaseListFilterType.String,
                Class = "filter-search-container",
                FilterPath = "locationName",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetFromDateFilter()
        {
            return new BaseListFilterDto
            {
                Name = "FromDateFilter",
                Label = "FromDate",
                PlaceHolder = "SelectFromDate",
                FilterType = BaseListFilterType.FromDate,
                Class = "filter-default-container",
                MinDate = DateTime.Now,
                MaxDate = DateTime.Now,
                FilterPath = "startTimeStart",
                DateFromValue = DateTime.Now,
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetToDateFilter()
        {
            return new BaseListFilterDto
            {
                Name = "ToDateFilter",
                Label = "ToDate",
                PlaceHolder = "SelectToDate",
                FilterType = BaseListFilterType.ToDate,
                Class = "filter-default-container",
                MinDate = DateTime.Now,
                MaxDate = DateTime.Now,
                FilterPath = "startTimeEnd",
                DateFromValue = DateTime.Now,
                DateToValue = DateTime.Now,
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetDateRangeFilter()
        {
            return new BaseListFilterDto
            {
                Name = "DateRangeFilter",
                Label = "DateRange",
                PlaceHolder = "SelectDateRange",
                FilterType = BaseListFilterType.DateRange,
                Class = "filter-default-container",
                MinDate = DateTime.Now,
                MaxDate = DateTime.Now,
                FilterPath = "dateRange",
                EventOnChange = "fetchRecords",
            };
        }

        public static BaseListFilterDto GetFromTimeFilter()
        {
            return new BaseListFilterDto
            {
                Name = "FromTimeFilter",
                Label = "FromTimeSearch",
                PlaceHolder = "SelectFromTime",
                FilterType = BaseListFilterType.FromTime,
                Class = "filter-timepicker-container",
                FilterPath = "startTimeStart",
                DateFromValue = DateTime.Now,
                EventOnChange = "fetchRecords"
            };
        }

        public static BaseListFilterDto GetToTimeFilter()
        {
            return new BaseListFilterDto
            {
                Name = "ToTimeFilter",
                Label = "ToTimeSearch",
                PlaceHolder = "SelectToTime",
                FilterType = BaseListFilterType.ToTime,
                Class = "filter-timepicker-container",
                FilterPath = "startTimeEnd",
                DateFromValue = DateTime.Now,
                DateToValue = DateTime.Now,
                EventOnChange = "fetchRecords"
            };
        }
        public static BaseListFilterDto GetSingleSelectFilter(string label, string filterPath)
        {
            return new BaseListFilterDto
            {
                Name = "SingleSelectFilter",
                FilterType = BaseListFilterType.SingleSelect,
                FilterPath = filterPath,
                Label = label,
                Class = "filter-default-container",
                FilterOptions = new List<BaseListDropdownOptionDto>()
            };
        }




    }
}