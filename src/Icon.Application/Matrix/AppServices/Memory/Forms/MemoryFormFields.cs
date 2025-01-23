using System;
using System.Collections.Generic;
using System.Linq;
using Icon.BaseManagement;
using Icon.Matrix.Portal.Dto;

namespace Icon.Matrix.Memories.Forms
{
    public static class MemoryFormFields
    {
        public static BaseFormFieldDto GetMemoryId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.MemoryId),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, Guid>(f => f.MemoryId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetCharacterId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.Character) + "." + nameof(MemoryFormModel.Character.Id),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, Guid?>(f => f.Character.Id),
            isDisabled: true,
            isHidden: true,
            isRequired: true
        );

        public static BaseFormFieldDto GetCharacterName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.Character) + "." + nameof(MemoryFormModel.Character.Name),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.Character.Name),
            isRequired: true,
            isDisabled: true
        );

        // personaid 
        public static BaseFormFieldDto GetPersonaId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.Persona) + "." + nameof(MemoryFormModel.Persona.Id),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, Guid>(f => f.Persona.Id),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetPersonaName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.Persona) + "." + nameof(MemoryFormModel.Persona.Name),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.Persona.Name),
            isRequired: true
        );

        // memorytype name
        public static BaseFormFieldDto GetMemoryTypeName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.MemoryType) + "." + nameof(MemoryFormModel.MemoryType.Name),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.MemoryType.Name),
            columnWidth: 6,
            isRequired: true
        );

        // platform name
        public static BaseFormFieldDto GetPlatformName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.Platform) + "." + nameof(MemoryFormModel.Platform.Name),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.Platform.Name),
            columnWidth: 6,
            isRequired: true
        );

        public static BaseFormFieldDto GetMemoryContent() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(MemoryFormModel.MemoryContent),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.MemoryContent),
            isRequired: true
        );

        public static BaseFormFieldDto GetMemoryUrl() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(MemoryFormModel.MemoryUrl),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.MemoryUrl)
        );

        // memory.memoryprompt.responseJson

        public static BaseFormFieldDto GetMemoryPromptOutput() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(MemoryFormModel.Prompt.PromptOutput),
            valuePath: BaseHelper.GetPropertyPath<MemoryFormModel, string>(f => f.Prompt.PromptOutput),
            isRequired: false
        );




    }
}