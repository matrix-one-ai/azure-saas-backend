using System;
using System.Collections.Generic;
using Abp.UI;

namespace Icon.BaseManagement
{
    public static class BaseFormFieldFactory
    {
        public static BaseFormFieldDto CreateTextField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            bool isHidden = false,
            bool isLabelHidden = false,
            bool shouldTranslate = false,
            string placeholder = "")
        {
            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.Text,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                IsHidden = isHidden,
                ShouldTranslate = shouldTranslate,
                IsLabelHidden = isLabelHidden,
                Placeholder = placeholder,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                Options = new BaseFormFieldOptionsDto(),

                OriginalValue = string.Empty,
                UpdatedValue = string.Empty,

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),
            };
        }

        // text area
        public static BaseFormFieldDto CreateTextAreaField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            bool isHidden = false,
            string placeholder = "")
        {
            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.TextArea,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                IsHidden = isHidden,
                Placeholder = placeholder,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                Options = new BaseFormFieldOptionsDto(),

                OriginalValue = string.Empty,
                UpdatedValue = string.Empty,

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),
            };
        }

        // number field
        public static BaseFormFieldDto CreateNumberField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            bool isHidden = false,
            string placeholder = "")
        {
            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.Number,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                IsHidden = isHidden,
                Placeholder = placeholder,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                Options = new BaseFormFieldOptionsDto(),

                // OriginalValue = string.Empty,
                // UpdatedValue = string.Empty,

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),
            };
        }

        // checkbox field
        public static BaseFormFieldDto CreateCheckboxField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            bool isHidden = false,
            string placeholder = "")
        {
            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.Checkbox,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                IsHidden = isHidden,
                Placeholder = placeholder,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                Options = new BaseFormFieldOptionsDto(),

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),
            };
        }


        public static BaseFormFieldDto CreateDateField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            bool isHidden = false,
            string placeholder = "",
            BaseFormFieldOptionsDto fieldOptions = null,
            string visibilityCondition = null,
            string enableCondition = null,
            string validationCondition = null)
        {
            if (!fieldName.Contains("Date"))
            {
                fieldName = fieldName.Replace("Time", "Date");
            }
            else
            {
                fieldName = fieldName.Replace("Time", "");
            }

            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.Date,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                IsHidden = isHidden,
                Placeholder = placeholder,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                Options = fieldOptions ?? new BaseFormFieldOptionsDto(),

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),
                ValidationCondition = validationCondition,
                EnableCondition = enableCondition,
                VisibilityCondition = visibilityCondition,
            };
        }

        public static BaseFormFieldDto CreateTimeField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            string placeholder = "",
            bool showPlaceHolder = false,
            BaseFormFieldOptionsDto fieldOptions = null)
        {
            fieldName = fieldName.Replace("Date", "");
            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.Time,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                Placeholder = placeholder,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                Options = fieldOptions ?? new BaseFormFieldOptionsDto
                {
                    HourPlaceHolder = isDisabled || !showPlaceHolder ? "" : "HH",
                    MinutePlaceHolder = isDisabled || !showPlaceHolder ? "" : "MM",
                },

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),
            };
        }

        public static BaseFormFieldDto CreateDropdownField(
            string fieldName,
            string valuePath,
            int columnWidth = 12,
            bool isRequired = false,
            bool isDisabled = false,
            bool isReadOnly = false,
            bool shouldTranslate = false,
            string placeholder = "",
            List<BaseFormDropdownOptionDto> options = null,
            string optionValuePath = "id",
            Guid? selectedValue = null)
        {

            // if (options == null || options.Count == 0)
            // {
            //     options = new List<BaseFormDropdownOptionDto>
            //     {
            //         new BaseFormDropdownOptionDto
            //         {
            //             Id = Guid.Empty,
            //             Name = "FormField.Option.Select",
            //             IsDisabled = true
            //         }
            //     };
            // }
            // else
            // {
            //     options.Insert(0, new BaseFormDropdownOptionDto
            //     {
            //         Id = Guid.Empty,
            //         Name = "FormField.Option.Select",
            //         IsDisabled = true
            //     });
            // }


            return new BaseFormFieldDto
            {
                FieldType = BaseFormFieldType.SingleSelect,
                ColumnWidth = columnWidth,

                FieldName = "FormField.Name." + fieldName,
                Label = "FormField.Label." + fieldName,
                IsRequired = isRequired,
                IsDisabled = isDisabled,
                IsReadOnly = isReadOnly,
                Placeholder = placeholder,
                ShouldTranslate = shouldTranslate,

                ShowPlaceHolder = !string.IsNullOrEmpty(placeholder),
                OptionValuePath = optionValuePath,
                Options = new BaseFormFieldOptionsDto
                {
                    DropdownOptions = options ?? new List<BaseFormDropdownOptionDto>()
                },

                BackendValuePath = valuePath,
                FrontEndValuePath = BaseHelper.ConvertPathToLowerCamelCase(valuePath),

                OriginalValue = selectedValue ?? Guid.Empty,
                UpdatedValue = selectedValue ?? Guid.Empty,
            };
        }

        // public static BaseFormFieldDto CreateDateTimeField(string label, string fieldName, bool isRequired = false, DateTime? minDate = null, DateTime? maxDate = null, int columnWidth = 12)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = label,
        //         FieldName = fieldName,
        //         FieldType = BaseFormFieldType.DateTime,
        //         IsRequired = isRequired,
        //         ValidationMessage = $"{label} is required.",
        //         Options = new BaseFormFieldOptionsDto
        //         {
        //             MinDate = minDate,
        //             MaxDate = maxDate
        //         },
        //         ColumnWidth = columnWidth,
        //         OriginalValue = null,  // DateTime value setup
        //         UpdatedValue = null    // DateTime value setup
        //     };
        // }

        // CLIENT
        // CreateExternalClientIdField
        // public static BaseFormFieldDto CreateExternalClientIdField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ExternalClientId",
        //         FieldName = "ExternalId",
        //         FieldType = BaseFormFieldType.Number,
        //         IsRequired = false,
        //         IsDisabled = true,
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // // CreateClientNumberField
        // public static BaseFormFieldDto CreateClientNumberField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ClientNumber",
        //         FieldName = "ClientNumber",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = true,
        //         ValidationMessage = "FormFieldValidation.ClientNumberIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // // CreateFirstNameField
        // public static BaseFormFieldDto CreateClientFirstNameField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ClientFirstName",
        //         FieldName = "FirstName",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = false,
        //         IsDisabled = true,
        //         ValidationMessage = "FormFieldValidation.FirstNameIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // // CreateLastNameField
        // public static BaseFormFieldDto CreateClientLastNameField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ClientLastName",
        //         FieldName = "LastName",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = false,
        //         IsDisabled = true,
        //         ValidationMessage = "FormFieldValidation.LastNameIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // // CreateGenderField
        // public static BaseFormFieldDto CreateGenderField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.Gender",
        //         FieldName = "Gender",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = true,
        //         ValidationMessage = "FormFieldValidation.GenderIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty,
        //     };
        // }

        // // CreateDateOfBirthField
        // public static BaseFormFieldDto CreateDateOfBirthField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.DateOfBirth",
        //         FieldName = "DateOfBirth",
        //         FieldType = BaseFormFieldType.Date,
        //         IsRequired = true,
        //         ValidationMessage = "FormFieldValidation.DateOfBirthIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = null,
        //         UpdatedValue = null
        //     };
        // }


        // // TRIP

        // public static BaseFormFieldDto CreateExternalTripIdField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ExternalTripID",
        //         FieldName = "ExternalId",
        //         FieldType = BaseFormFieldType.Number,
        //         IsRequired = false,
        //         IsDisabled = true,
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateScheduledStartDateField(int columnWidth = 4)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromDate",
        //         FieldName = "ScheduledStartTime",
        //         Placeholder = "FormField.Placeholder.SelectStartDate",
        //         FieldType = BaseFormFieldType.Date,
        //         IsRequired = true,
        //         ValidationMessage = "FormFieldValidation.ScheduledStartDateIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = null,
        //         UpdatedValue = null
        //     };
        // }

        // public static BaseFormFieldDto CreateScheduledEndDateField(int columnWidth = 4)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToDate",
        //         FieldName = "ScheduledEndTime",
        //         Placeholder = "FormField.Placeholder.SelectEndDate",
        //         FieldType = BaseFormFieldType.Date,
        //         IsRequired = true,
        //         ValidationMessage = "FormFieldValidation.ScheduledEndDateIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = null,
        //         UpdatedValue = null
        //     };
        // }

        // public static BaseFormFieldDto CreateScheduledStartTimeField(int columnWidth = 2)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromTime",
        //         FieldName = "ScheduledStartTime",
        //         Placeholder = "FormField.Placeholder.SelectStartTime",
        //         FieldType = BaseFormFieldType.Time,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.ScheduledStartTimeIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = null,
        //         UpdatedValue = null
        //     };
        // }

        // public static BaseFormFieldDto CreateScheduledEndTimeField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToTime",
        //         FieldName = "ScheduledEndTime",
        //         Placeholder = "FormField.Placeholder.SelectEndTime",
        //         FieldType = BaseFormFieldType.Time,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.ScheduledEndTimeIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = null,
        //         UpdatedValue = null
        //     };
        // }

        // public static BaseFormFieldDto CreateFromStreetField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromStreet",
        //         FieldName = "FromStreet",
        //         Placeholder = "FormField.Placeholder.EnterFromStreet",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.FromStreetIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateFromStreetNumberField(int columnWidth = 3)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromStreetNumber",
        //         FieldName = "FromStreetNumber",
        //         Placeholder = "FormField.Placeholder.EnterFromStreetNumber",
        //         FieldType = BaseFormFieldType.Number,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.FromStreetNumberIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateFromStreetNumberExtraField(int columnWidth = 3)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromStreetNumberExtra",
        //         FieldName = "FromStreetNumberExtra",
        //         // Placeholder = "FormField.Placeholder.EnterFromStreetNumberExtra",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = false,
        //         ValidationMessage = "FormField.Validation.FromStreetNumberExtraIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateFromZipCodeField(int columnWidth = 4)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromZipCode",
        //         FieldName = "FromZipCode",
        //         Placeholder = "FormField.Placeholder.EnterFromZipCode",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = false,
        //         ValidationMessage = "FormField.Validation.FromZipCodeIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateFromCityField(int columnWidth = 4)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.FromCity",
        //         FieldName = "FromCity",
        //         Placeholder = "FormField.Placeholder.EnterFromCity",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.FromCityIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateToStreetField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToStreet",
        //         FieldName = "ToStreet",
        //         Placeholder = "FormField.Placeholder.EnterToStreet",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.ToStreetIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateToStreetNumberField(int columnWidth = 3)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToStreetNumber",
        //         FieldName = "ToStreetNumber",
        //         Placeholder = "FormField.Placeholder.EnterToStreetNumber",
        //         FieldType = BaseFormFieldType.Number,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.ToStreetNumberIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateToStreetNumberExtraField(int columnWidth = 3)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToStreetNumberExtra",
        //         FieldName = "ToStreetNumberExtra",
        //         Placeholder = "FormField.Placeholder.EnterToStreetNumberExtra",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = false,
        //         ValidationMessage = "FormField.Validation.ToStreetNumberExtraIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateToZipCodeField(int columnWidth = 4)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToZipCode",
        //         FieldName = "ToZipCode",
        //         Placeholder = "FormField.Placeholder.EnterToZipCode",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = false,
        //         ValidationMessage = "FormField.Validation.ToZipCodeIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateToCityField(int columnWidth = 4)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         Label = "FormField.Label.ToCity",
        //         FieldName = "ToCity",
        //         Placeholder = "FormField.Placeholder.EnterToCity",
        //         FieldType = BaseFormFieldType.Text,
        //         IsRequired = true,
        //         ValidationMessage = "FormField.Validation.ToCityIsRequired",
        //         ColumnWidth = columnWidth,
        //         OriginalValue = string.Empty,
        //         UpdatedValue = string.Empty
        //     };
        // }

        // public static BaseFormFieldDto CreateClientBlockadeTypeField(int columnWidth = 6)
        // {
        //     return new BaseFormFieldDto
        //     {
        //         FieldName = "ClientBlockadeType",
        //         Label = "FormField.Label.ClientBlockadeType",
        //         FieldType = BaseFormFieldType.SingleSelect,
        //         IsRequired = true,
        //         IsReadOnly = false,
        //         IsDisabled = false,
        //         Placeholder = "FormField.PlaceHolder.ClientBlockadeType",
        //         Options = new BaseFormFieldOptionsDto
        //         {
        //             DropdownOptions = new List<BaseFormDropdownOptionDto>
        //             {
        //                 new BaseFormDropdownOptionDto
        //                 {
        //                     Id = ClientBlockadeTypeConst.Temporary.ToString(),
        //                     Name = ClientBlockadeTypeConst.Name_Temporary
        //                 },
        //                 new BaseFormDropdownOptionDto
        //                 {
        //                     Id = ClientBlockadeTypeConst.Definitive.ToString(),
        //                     Name = ClientBlockadeTypeConst.Name_Definitive
        //                 },
        //                 new BaseFormDropdownOptionDto
        //                 {
        //                     Id = ClientBlockadeTypeConst.Sick.ToString(),
        //                     Name = ClientBlockadeTypeConst.Name_Sick
        //                 }
        //             }
        //         }
        //     };
        // }

    }
}