// aspnet-core/src/Icon.Application/Matrix/Characters/Forms/CharacterFormFields.cs

using System;
using Icon.BaseManagement;

namespace Icon.Matrix.Characters.Forms
{
    public static partial class CharacterFormFields
    {
        public static BaseFormFieldDto GetCharacterId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterFormModel.CharacterId),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, Guid>(f => f.CharacterId)
        );

        public static BaseFormFieldDto GetCharacterName() => BaseFormFieldFactory.CreateTextField(
            fieldName: "Character" + nameof(CharacterFormModel.Name),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.Name),
            isRequired: true
        );

        public static BaseFormFieldDto GetTwitterPostAgentId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterFormModel.TwitterPostAgentId),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.TwitterPostAgentId),
            isRequired: false,
            columnWidth: 6

        );

        public static BaseFormFieldDto GetTwitterScrapeAgentId() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterFormModel.TwitterScrapeAgentId),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.TwitterScrapeAgentId),
            isRequired: false,
            columnWidth: 6
        );

        public static BaseFormFieldDto GetIsTwitterScrapingEnabled() => BaseFormFieldFactory.CreateCheckboxField(
            fieldName: nameof(CharacterFormModel.IsTwitterScrapingEnabled),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, bool>(f => f.IsTwitterScrapingEnabled),
            columnWidth: 6

        );

        public static BaseFormFieldDto GetIsTwitterPostingEnabled() => BaseFormFieldFactory.CreateCheckboxField(
            fieldName: nameof(CharacterFormModel.IsTwitterPostingEnabled),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, bool>(f => f.IsTwitterPostingEnabled),
            columnWidth: 6

        );

        public static BaseFormFieldDto GetIsPromptingEnabled() => BaseFormFieldFactory.CreateCheckboxField(
            fieldName: nameof(CharacterFormModel.IsPromptingEnabled),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, bool>(f => f.IsPromptingEnabled)
        );

        public static BaseFormFieldDto GetTwitterUserName() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterFormModel.TwitterUserName),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.TwitterUserName),
            isRequired: false
        );

        // -- Bio Fields --

        public static BaseFormFieldDto GetBio() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Bio),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Bio),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetPersonality() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Personality),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Personality),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetAppearance() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Appearance),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Appearance),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetOccupation() => BaseFormFieldFactory.CreateTextField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Occupation),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Occupation),
            isRequired: false
        );

        public static BaseFormFieldDto GetBirthDate() => BaseFormFieldFactory.CreateDateField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.BirthDate),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, DateTimeOffset>(f => f.CurrentBio.BirthDate),
            isRequired: false
        );

        public static BaseFormFieldDto GetActiveFrom() => BaseFormFieldFactory.CreateDateField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.ActiveFrom),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, DateTimeOffset>(f => f.CurrentBio.ActiveFrom),
            isRequired: false,
            columnWidth: 6
        );

        public static BaseFormFieldDto GetActiveTo() => BaseFormFieldFactory.CreateDateField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.ActiveTo),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, DateTimeOffset?>(f => f.CurrentBio.ActiveTo),
            isRequired: false,
            columnWidth: 6
        );

        public static BaseFormFieldDto GetIsActive() => BaseFormFieldFactory.CreateCheckboxField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.IsActive),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, bool>(f => f.CurrentBio.IsActive)
        );

        public static BaseFormFieldDto GetMotivations() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Motivations),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Motivations),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetFears() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Fears),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Fears),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetValues() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Values),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Values),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetSpeechPatterns() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.SpeechPatterns),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.SpeechPatterns),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetSkills() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Skills),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Skills),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetBackstory() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Backstory),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Backstory),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetPublicPersona() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.PublicPersona),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.PublicPersona),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetPrivateSelf() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.PrivateSelf),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.PrivateSelf),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetMediaPresence() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.MediaPresence),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.MediaPresence),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetCrisisBehavior() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.CrisisBehavior),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.CrisisBehavior),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetRelationships() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.Relationships),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.Relationships),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetTechDetails() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.CurrentBio) + "." + nameof(CharacterBio.TechDetails),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.CurrentBio.TechDetails),
            isRequired: false,
            columnWidth: 12
        );

        // -- Prompt Instructions Fields --

        public static BaseFormFieldDto GetPromptInstruction() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.PromptInstructions) + "." + nameof(CharacterPromptInstruction.PromptInstruction),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.PromptInstructions.PromptInstruction),
            isRequired: false,
            columnWidth: 12
        );

        public static BaseFormFieldDto GetOutputExamples() => BaseFormFieldFactory.CreateTextAreaField(
            fieldName: nameof(CharacterFormModel.PromptInstructions) + "." + nameof(CharacterPromptInstruction.OutputExamples),
            valuePath: BaseHelper.GetPropertyPath<CharacterFormModel, string>(f => f.PromptInstructions.OutputExamples),
            isRequired: false,
            columnWidth: 12
        );
    }
}
