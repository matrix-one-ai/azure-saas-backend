using System;
using System.Collections.Generic;

namespace Icon.BaseManagement
{
    public class BaseFormFieldDto
    {
        public string Label { get; set; }
        public string FieldName { get; set; }
        public bool ShowPlaceHolder { get; set; }
        public string Placeholder { get; set; }
        public BaseFormFieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public string ValidationMessage { get; set; }
        public string FrontEndValuePath { get; set; }
        public string BackendValuePath { get; set; }
        public object OriginalValue { get; set; }
        public object UpdatedValue { get; set; }
        public DateTime? OriginalDateTimeValue { get; set; }
        public DateTime? UpdatedDateTimeValue { get; set; }
        public string OptionValuePath { get; set; }
        public BaseFormFieldOptionsDto Options { get; set; }
        public string Class { get; set; }
        public bool IsDisabled { get; set; }
        public bool IsHidden { get; set; }
        public bool IsReadOnly { get; set; }
        public bool ShouldTranslate { get; set; }
        public bool IsLabelHidden { get; set; }
        public string Tooltip { get; set; }
        public int ColumnWidth { get; set; } = 12; // Default to full width

        public string VisibilityCondition { get; set; }
        public string EnableCondition { get; set; }
        public string ValidationCondition { get; set; }
        public string GetStringValue()
        {
            return UpdatedValue?.ToString();
        }

        public DateTime? GetDateValue()
        {
            return UpdatedValue as DateTime?;
        }

        public int? GetIntValue()
        {
            return UpdatedValue as int?;
        }

        public void SetStringValue(string value)
        {
            UpdatedValue = value;
        }

        public void SetDateValue(DateTime? value)
        {
            UpdatedValue = value;
        }

        public void SetIntValue(int? value)
        {
            UpdatedValue = value;
        }

        public void Disable()
        {
            IsDisabled = true;
        }
    }

    public class BaseFormFieldOptionsDto
    {
        public List<BaseFormDropdownOptionDto> DropdownOptions { get; set; } = new List<BaseFormDropdownOptionDto>();
        //public List<BaseFormSimpleDropdownOptionDto> SimpleDropdownOptions { get; set; } = new List<BaseFormSimpleDropdownOptionDto>();
        public int MaxLength { get; set; }
        public int MinLength { get; set; }
        public double? MinValue { get; set; }
        public double? MaxValue { get; set; }
        public DateTime? MinDate { get; set; }
        public DateTime? MaxDate { get; set; }
        public string RegexPattern { get; set; }
        public string CustomValidator { get; set; }
        public string HourPlaceHolder { get; set; }
        public string MinutePlaceHolder { get; set; }
    }

    public class BaseFormDropdownOptionDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsDisabled { get; set; }
    }

    // public class BaseFormSimpleDropdownOptionDto
    // {
    //     public string Id { get; set; }
    //     public string Name { get; set; }
    //     public bool IsDisabled { get; set; }
    // }

}