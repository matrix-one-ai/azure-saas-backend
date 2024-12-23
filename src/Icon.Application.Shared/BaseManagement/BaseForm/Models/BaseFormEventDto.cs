using System;
using System.Collections.Generic;

namespace Icon.BaseManagement
{
    public class BaseFormEventDto
    {
        public string Event { get; set; }
        public BaseFormEventType EventType { get; set; }
        public string UpdateToStringValue { get; set; }
        public DateTime? UpdateToDateTimeValue { get; set; }
        public int? UpdateToIntValue { get; set; }
    }

    public class BaseFormEventSetValueDto
    {
        public string TargetFieldName { get; set; }
        public string SetToStringValue { get; set; }
        public DateTime? SetToDateTimeValue { get; set; }
        public int? SetToIntValue { get; set; }
    }


    public enum BaseFormEventType
    {
        DisableField,
        EnableField,
        HideField,
        ShowField,
        SetValue,
        SetMinValue,
        SetMaxValue,
    }

    public enum BaseFormEventTriggerType
    {
        ValueChange,
        FieldFocus,
        FieldBlur,
        FieldClick,
        FieldDoubleClick,
        FieldMouseEnter,
        FieldMouseLeave,
        FieldKeyPress,
        FieldKeyUp,
        FieldKeyDown,
        FieldSubmit,
        FormSubmit,
    }

}