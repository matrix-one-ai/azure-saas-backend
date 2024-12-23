using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Abp.Localization;
using Abp.UI;

namespace Icon.BaseManagement
{
    public class BaseFormDto
    {
        private ILocalizationManager _localizationManager;

        public string FormTitle { get; set; }
        public Guid? LocationId { get; set; }
        public string EntityName { get; set; }
        public Guid? EntityId { get; set; }
        public string FormIcon { get; set; }
        public bool ShowValidationSummary { get; set; }
        public Type FormClass { get; set; }
        // public List<BaseFormSectionDto> Sections { get; set; } = new List<BaseFormSectionDto>();
        private List<BaseFormSectionDto> _sections = new List<BaseFormSectionDto>();
        public List<BaseFormSectionDto> Sections
        {
            get => _sections;
            set => _sections = value?.Where(section => section != null).ToList() ?? new List<BaseFormSectionDto>();
        }
        public List<BaseSettingEnum> Settings { get; set; } = new List<BaseSettingEnum>();

        public BaseFormDto()
        {
        }

        public BaseFormDto(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        public BaseFormFieldDto GetFormField(string fieldName)
        {
            foreach (var section in Sections)
            {
                if (section == null || section.Rows == null) continue;
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (field.FieldName == fieldName)
                        {
                            return field;
                        }
                    }
                }
            }

            return null;
        }

        public T GetFormFieldValue<T>(string fieldName)
        {
            var field = GetFormField(fieldName);
            if (field == null)
            {
                throw new UserFriendlyException($"BaseForm {FormTitle} - Field '{fieldName}' not found.");
            }

            if (field.FieldType == BaseFormFieldType.SingleSelect)
            {
                return (T)Convert.ChangeType(field.UpdatedValue, typeof(T));
            }

            return (T)field.UpdatedValue;
        }

        public BaseFormSectionDto BuildSection(string sectionTitle, List<BaseFormRowDto> rows)
        {
            return new BaseFormSectionDto
            {
                SectionTitle = sectionTitle,
                Rows = rows
            };
        }

        public BaseFormRowDto BuildRow(List<BaseFormFieldDto> fields)
        {
            return new BaseFormRowDto
            {
                Fields = fields
            };
        }

        public void DisableFields(List<string> fieldNames)
        {
            foreach (var section in Sections)
            {
                if (section == null || section.Rows == null) continue;
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (fieldNames.Contains(field.FieldName))
                        {
                            field.IsDisabled = true;
                        }
                    }
                }
            }
        }

        public void EnableFields(List<string> fieldNames)
        {
            foreach (var section in Sections)
            {
                if (section == null || section.Rows == null) continue;
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (fieldNames.Contains(field.FieldName))
                        {
                            field.IsDisabled = false;
                        }
                    }
                }
            }
        }

        public void HideFields(List<string> fieldNames)
        {
            foreach (var section in Sections)
            {
                if (section == null || section.Rows == null) continue;
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (fieldNames.Contains(field.FieldName))
                        {
                            field.IsHidden = true;
                        }
                    }
                }
            }
        }

        public void ApplySettings()
        {
            foreach (var section in Sections)
            {
                foreach (var row in section.Rows)
                {
                    if (row.Fields == null) throw new UserFriendlyException($"BaseForm {FormTitle} - Fields cannot be null");

                    foreach (var field in row.Fields)
                    {

                        if (field == null) throw new UserFriendlyException($"BaseForm {FormTitle} - Field cannot be null");

                        if (Settings.Contains(BaseSettingEnum.ReadOnlyAll)) field.IsReadOnly = true;
                        else if (Settings.Contains(BaseSettingEnum.ReadOnlyNone)) field.IsReadOnly = false;
                        else if (Settings.Contains(BaseSettingEnum.ReadOnlySome)) field.IsReadOnly = field.IsReadOnly;

                        if (Settings.Contains(BaseSettingEnum.DisableAll)) field.IsDisabled = true;
                        else if (Settings.Contains(BaseSettingEnum.DisableNone)) field.IsDisabled = false;
                        else if (Settings.Contains(BaseSettingEnum.DisableSome)) field.IsDisabled = field.IsDisabled;

                        if (Settings.Contains(BaseSettingEnum.RequireAll)) field.IsRequired = true;
                        else if (Settings.Contains(BaseSettingEnum.RequireNone)) field.IsRequired = false;
                        else if (Settings.Contains(BaseSettingEnum.RequireSome)) field.IsRequired = field.IsRequired;

                        if (Settings.Contains(BaseSettingEnum.ShowPlaceHolderAll)) field.ShowPlaceHolder = true;
                        else if (Settings.Contains(BaseSettingEnum.ShowPlaceHolderNone)) field.ShowPlaceHolder = false;
                        else if (Settings.Contains(BaseSettingEnum.ShowPlaceHolderSome)) field.ShowPlaceHolder = field.ShowPlaceHolder;
                    }
                }
            }
        }

        public void SetFieldValues(object dto)
        {
            if (dto == null) throw new ArgumentNullException(nameof(dto));

            var dtoType = dto.GetType();
            foreach (var section in Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (string.IsNullOrEmpty(field.BackendValuePath))
                        {
                            continue; // Skip fields that don't have a value path defined
                        }

                        var propertyValue = GetPropertyValueByPath(dto, field.BackendValuePath);
                        if (propertyValue == null)
                        {
                            continue;
                        }

                        if ((field.FieldType == BaseFormFieldType.Date || field.FieldType == BaseFormFieldType.Time) && propertyValue is DateTime dateTimeValue)
                        {
                            field.OriginalDateTimeValue = dateTimeValue;
                            field.UpdatedDateTimeValue = dateTimeValue;

                            var formattedDate = dateTimeValue.ToString("yyyy-MM-ddTHH:mm:ss");
                            field.OriginalValue = formattedDate;
                            field.UpdatedValue = formattedDate;
                        }
                        else
                        {
                            field.OriginalValue = propertyValue;
                            field.UpdatedValue = propertyValue;
                            field.OriginalDateTimeValue = null;
                            field.UpdatedDateTimeValue = null;
                        }

                        if (field.FieldType == BaseFormFieldType.SingleSelect)
                        {
                            if (field.ShouldTranslate)
                            {
                                foreach (var option in field.Options?.DropdownOptions)
                                {
                                    option.Name = _localizationManager.GetString(IconConsts.LocalizationSourceName, option.Name);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void SetLocalization(ILocalizationManager localizationManager)
        {
            _localizationManager = localizationManager;
        }

        public void ValidateRequiredFields()
        {
            if (_localizationManager == null) throw new UserFriendlyException("BaseFormDto - LocalizationManager cannot be null");
            var localizationSourceName = _localizationManager.GetSource(IconConsts.LocalizationSourceName);

            var invalidFields = new List<BaseFormFieldDto>();
            foreach (var section in Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (field.IsRequired && field.UpdatedValue == null && field.UpdatedDateTimeValue == null)
                        {
                            invalidFields.Add(field);
                        }
                        else if (field.IsRequired && field.UpdatedValue is string && string.IsNullOrWhiteSpace(field.UpdatedValue.ToString()))
                        {
                            invalidFields.Add(field);
                        }
                        else if (field.IsRequired && field.UpdatedValue.ToString() == Guid.Empty.ToString())
                        {
                            invalidFields.Add(field);
                        }
                        // else if (field.IsRequired && field.UpdatedValue is Guid? && (field.UpdatedValue == null || (Guid)field.UpdatedValue == Guid.Empty))
                        // {
                        //     invalidFields.Add(field);
                        // }
                    }
                }
            }

            if (invalidFields.Any())
            {
                throw new UserFriendlyException(
                    message: _localizationManager.GetString(
                         new LocalizableString(
                            name: "Error.FormValidation.RequiredFieldsAreEmpty",
                            sourceName: localizationSourceName.Name)).ToString(),
                    details: invalidFields.Select(f =>
                        _localizationManager.GetString(IconConsts.LocalizationSourceName, f.Label)).Aggregate((i, j) => i + ", " + j));
            }
        }

        private object GetPropertyValueByPath(object obj, string path)
        {
            var properties = path.Split('.');
            foreach (var property in properties)
            {
                if (obj == null)
                {
                    return null; // Return null if any parent property is null
                }

                var propertyInfo = obj.GetType().GetProperty(property, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null)
                {
                    return null; // Return null if the property doesn't exist
                }

                obj = propertyInfo.GetValue(obj);
            }
            return obj;
        }

        public void UpdateDtoFromFieldValues(object dto)
        {
            if (dto == null) throw new UserFriendlyException("BaseForm - UpdateDtoFromFieldValues - DTO cannot be null");

            var dateFields = new List<BaseFormFieldType>
            {
                BaseFormFieldType.Date,
                BaseFormFieldType.Time,
                BaseFormFieldType.DateTime
            };

            if (Sections == null) throw new UserFriendlyException("BaseForm - UpdateDtoFromFieldValues - Form Sections cannot be null");

            foreach (var section in Sections)
            {
                foreach (var row in section.Rows)
                {
                    foreach (var field in row.Fields)
                    {
                        if (string.IsNullOrEmpty(field.BackendValuePath))
                        {
                            continue;
                        }

                        if (dateFields.Contains(field.FieldType))
                        {
                            SetPropertyValueByPath(dto, field.BackendValuePath, field.UpdatedDateTimeValue);
                        }
                        else
                        {
                            SetPropertyValueByPath(dto, field.BackendValuePath, field.UpdatedValue);
                        }
                    }
                }
            }
        }

        private void SetPropertyValueByPath(object obj, string path, object value)
        {
            if (obj == null || string.IsNullOrEmpty(path)) return;

            var properties = path.Split('.');
            for (int i = 0; i < properties.Length - 1; i++)
            {
                var propertyInfo = obj.GetType().GetProperty(properties[i], BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                if (propertyInfo == null) return;

                var propertyValue = propertyInfo.GetValue(obj);
                if (propertyValue == null)
                {
                    propertyValue = Activator.CreateInstance(propertyInfo.PropertyType);
                    propertyInfo.SetValue(obj, propertyValue);
                }
                obj = propertyValue;
            }

            var finalProperty = obj.GetType().GetProperty(properties.Last(), BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            if (finalProperty != null && finalProperty.CanWrite)
            {
                if (value == null)
                {
                    if (finalProperty.PropertyType.IsValueType && Nullable.GetUnderlyingType(finalProperty.PropertyType) == null)
                    {
                        // If the property is a non-nullable value type (like int, Guid, DateTime), we can't set it to null
                        throw new InvalidCastException($"Cannot set null to a non-nullable value type: {finalProperty.PropertyType.Name}, for property: {finalProperty.Name}");
                    }
                    finalProperty.SetValue(obj, null); // Set nullable properties to null
                }
                else if (finalProperty.PropertyType == typeof(Guid) || finalProperty.PropertyType == typeof(Guid?))
                {
                    // Handle empty or null strings for Guid properties
                    if (string.IsNullOrEmpty(value.ToString()))
                    {
                        // Set to Guid.Empty or null depending on the target property type
                        finalProperty.SetValue(obj, finalProperty.PropertyType == typeof(Guid?) ? (Guid?)null : Guid.Empty);
                    }
                    else if (Guid.TryParse(value.ToString(), out var parsedGuid))
                    {
                        finalProperty.SetValue(obj, parsedGuid);
                    }
                    else
                    {
                        throw new InvalidCastException($"Unable to convert value '{value}' to Guid.");
                    }
                }
                else if (finalProperty.PropertyType.IsEnum)
                {
                    if (value is long || value is int)
                    {
                        // Attempt to cast the value to the appropriate enum type
                        finalProperty.SetValue(obj, Enum.ToObject(finalProperty.PropertyType, value));
                    }
                    else
                    {
                        throw new InvalidCastException($"Cannot convert value '{value}' to enum type {finalProperty.PropertyType.Name}.");
                    }
                }
                else
                {
                    // Convert the value to the target property type if necessary
                    var convertedValue = Convert.ChangeType(value, Nullable.GetUnderlyingType(finalProperty.PropertyType) ?? finalProperty.PropertyType);
                    finalProperty.SetValue(obj, convertedValue);
                }
            }
        }







    }


    public enum BaseFormType
    {
        PlaceHolder,
        CharacterPersonaCreate,
        CharacterPersonaEdit,
        CharacterPersonaDelete,
        CharacterPersonaView,

        MemoryView,
        MemoryEdit



    }

    public class BaseFormSectionDto
    {
        public string SectionTitle { get; set; }
        public bool IsHidden { get; set; }
        public List<BaseFormRowDto> Rows { get; set; } = new List<BaseFormRowDto>();
        public int ColumnWidth { get; set; } = 12;

        public void HideSection()
        {
            IsHidden = true;
        }

        public void HideFields(params string[] fieldNames)
        {
            foreach (var row in Rows)
            {
                foreach (var field in row.Fields)
                {
                    if (fieldNames.Contains(field.FieldName))
                    {
                        field.IsHidden = true;
                    }
                }
            }
        }

        public BaseFormSectionDto DisableAllFields()
        {
            foreach (var row in Rows)
            {
                foreach (var field in row.Fields)
                {
                    field.IsDisabled = true;
                }
            }

            return this;
        }

        public BaseFormSectionDto EnableAllFields()
        {
            foreach (var row in Rows)
            {
                foreach (var field in row.Fields)
                {
                    field.IsDisabled = false;
                }
            }

            return this;
        }

    }

    public class BaseFormRowDto
    {
        public List<BaseFormFieldDto> Fields { get; set; } = new List<BaseFormFieldDto>();
    }


    public class BaseFormButtonDto
    {
        public string Label { get; set; }
        public string Icon { get; set; }
        public string Event { get; set; }
        public string Class { get; set; }
    }


    public enum BaseFormFieldType
    {
        Text,
        TextArea,
        Number,
        Date,
        Time,
        DateTime, // Combines Date and Time into one field
        SingleSelect,
        Checkbox,
        Radio,
        Email,
        Password,
        File
    }
}
