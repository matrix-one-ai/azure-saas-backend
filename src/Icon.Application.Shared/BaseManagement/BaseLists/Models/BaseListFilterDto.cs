using System;
using Abp.Domain.Entities.Auditing;
using Abp.Domain.Entities;
using System.Collections.Generic;

namespace Icon.BaseManagement
{

    public class BaseListFilterDto
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string PlaceHolder { get; set; }
        public string Class { get; set; }
        public BaseListFilterType FilterType { get; set; }
        public string EventOnChange { get; set; }

        // Filter paths
        public string FilterPath { get; set; }
        public string FilterPathFrom { get; set; }
        public string FilterPathTo { get; set; }


        public string StringValue { get; set; }
        public double? NumberValue { get; set; }
        public DateTime? DateFromValue { get; set; }
        public DateTime? DateToValue { get; set; }
        // public List<DateTime?> DateRangeValue { get; set; }

        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }

        public Guid? DropdownValue { get; set; }
        public List<Guid> DropdownValues { get; set; } = new List<Guid>();
        public List<BaseListDropdownOptionDto> FilterOptions { get; set; } = new List<BaseListDropdownOptionDto>();
    }



    public enum BaseListFilterType
    {
        String,
        Number,
        FromDate,
        ToDate,
        DateRange,
        FromTime,
        ToTime,
        SingleSelect,
        MultiSelect,
        Button
    }



    public class BaseListFilterButtonDto
    {
        public string Name { get; set; }
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Event { get; set; }
        public string Class { get; set; }
    }

    public static class FilterButtonOptionsDto
    {
        public static readonly BaseListFilterButtonDto Reset = new BaseListFilterButtonDto
        {
            Name = "ResetButton",
            Label = "Reset",
            Icon = BaseIconOptions.Reset,
            Class = "btn btn-light-danger btn-outline-danger",
            Event = "resetFilters"
        };

        public static readonly BaseListFilterButtonDto Search = new BaseListFilterButtonDto
        {
            Name = "SearchButton",
            Label = "Search",
            Icon = BaseIconOptions.Search,
            Class = "btn btn-light-primary btn-outline-primary",
            Event = "fetchRecords"
        };

        public static List<BaseListFilterButtonDto> GetAllButtons()
        {
            return new List<BaseListFilterButtonDto> { Reset, Search };
        }
    }

    public class BaseListDropdownOptionDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
    }



}