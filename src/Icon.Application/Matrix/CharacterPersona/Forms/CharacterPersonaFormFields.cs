using System;
using System.Collections.Generic;
using System.Linq;
using Icon.BaseManagement;

namespace Icon.Matrix.CharacterPersonas.Forms
{
    public static partial class CharacterPersonaFormFields
    {
        public static BaseFormFieldDto GetCharacterPersonaId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.CharacterPersonaId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.CharacterPersonaId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetSelectCharacter(List<BaseFormDropdownOptionDto> options) => BaseFormFieldFactory.CreateDropdownField(
            fieldName: nameof(CharacterPersonaFormModel.Character) + "." + nameof(CharacterPersonaFormModel.Character.Id),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid?>(f => f.Character.Id),
            columnWidth: 12,
            shouldTranslate: false,
            isRequired: true,
            options: options.OrderBy(x => x.Name).ToList()
        );

        public static BaseFormFieldDto GetCharacterId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Character) + "." + nameof(CharacterPersonaFormModel.Character.Id),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid?>(f => f.Character.Id),
            isDisabled: true,
            isHidden: true,
            isRequired: true
        );

        public static BaseFormFieldDto GetCharacterName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Character) + "." + nameof(CharacterPersonaFormModel.Character.Name),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Character.Name),
            isRequired: true,
            isDisabled: true
        );

        // personaid 
        public static BaseFormFieldDto GetPersonaId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Persona) + "." + nameof(CharacterPersonaFormModel.Persona.Id),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.Persona.Id),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetPersonaName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Persona) + "." + nameof(CharacterPersonaFormModel.Persona.Name),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Persona.Name),
            isRequired: true
        );

        public static BaseFormFieldDto GetAttitude() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterPersonaFormModel.Attitude),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Attitude),
            isRequired: false
        );
        public static BaseFormFieldDto GetResponses() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterPersonaFormModel.Responses),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Responses),
            isRequired: false
        );

        public static BaseFormFieldDto GetTwitterPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Twitter) + "." + nameof(CharacterPersonaFormModel.Twitter.PlatformId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.Twitter.PlatformId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetTwitterPlatformName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Twitter) + "." + nameof(CharacterPersonaFormModel.Twitter.PlatformName),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Twitter.PlatformName),
            columnWidth: 3,
            isDisabled: true,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetTwitterPersonaPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Twitter) + "." + nameof(CharacterPersonaFormModel.Twitter.PlatformPersonaId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Twitter.PlatformPersonaId),
            columnWidth: 9,
            isDisabled: false,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetFacebookPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Facebook) + "." + nameof(CharacterPersonaFormModel.Facebook.PlatformId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.Facebook.PlatformId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetFacebookPlatformName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Facebook) + "." + nameof(CharacterPersonaFormModel.Facebook.PlatformName),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Facebook.PlatformName),
            columnWidth: 3,
            isDisabled: true,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetFacebookPersonaPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Facebook) + "." + nameof(CharacterPersonaFormModel.Facebook.PlatformPersonaId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Facebook.PlatformPersonaId),
            columnWidth: 9,
            isDisabled: false,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetInstagramPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Instagram) + "." + nameof(CharacterPersonaFormModel.Instagram.PlatformId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.Instagram.PlatformId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetInstagramPlatformName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Instagram) + "." + nameof(CharacterPersonaFormModel.Instagram.PlatformName),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Instagram.PlatformName),
            columnWidth: 3,
            isDisabled: true,
            isHidden: false,
            isLabelHidden: true
        );


        public static BaseFormFieldDto GetInstagramPersonaPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Instagram) + "." + nameof(CharacterPersonaFormModel.Instagram.PlatformPersonaId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Instagram.PlatformPersonaId),
            columnWidth: 9,
            isDisabled: false,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetDiscordPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Discord) + "." + nameof(CharacterPersonaFormModel.Discord.PlatformId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.Discord.PlatformId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetDiscordPlatformName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Discord) + "." + nameof(CharacterPersonaFormModel.Discord.PlatformName),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Discord.PlatformName),
            columnWidth: 3,
            isDisabled: true,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetDiscordPersonaPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Discord) + "." + nameof(CharacterPersonaFormModel.Discord.PlatformPersonaId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Discord.PlatformPersonaId),
            columnWidth: 9,
            isDisabled: false,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetTelegramPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Telegram) + "." + nameof(CharacterPersonaFormModel.Telegram.PlatformId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, Guid>(f => f.Telegram.PlatformId),
            isDisabled: true,
            isHidden: true
        );

        public static BaseFormFieldDto GetTelegramPlatformName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Telegram) + "." + nameof(CharacterPersonaFormModel.Telegram.PlatformName),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Telegram.PlatformName),
            columnWidth: 3,
            isDisabled: true,
            isHidden: false,
            isLabelHidden: true
        );

        public static BaseFormFieldDto GetTelegramPersonaPlatformId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterPersonaFormModel.Telegram) + "." + nameof(CharacterPersonaFormModel.Telegram.PlatformPersonaId),
            valuePath: BaseHelper.GetPropertyPath<CharacterPersonaFormModel, string>(f => f.Telegram.PlatformPersonaId),
            columnWidth: 9,
            isDisabled: false,
            isHidden: false,
            isLabelHidden: true
        );

    }
}