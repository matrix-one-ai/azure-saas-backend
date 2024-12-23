using System;
using System.Collections.Generic;
using Icon.BaseManagement;
using Icon.Matrix.Portal.Dto;

namespace Icon.Matrix.CharacterPersonas.Forms
{
    public static partial class CharacterPersonaForm
    {
        public static BaseFormSectionDto GetSetupSection() => new BaseFormSectionDto
        {
            SectionTitle = "Setup",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterPersonaFormFields.GetCharacterPersonaId(),
                    }
                }
            },
            IsHidden = true
        };

        public static BaseFormSectionDto GetCharacterSection() => new BaseFormSectionDto
        {
            SectionTitle = "Character",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterPersonaFormFields.GetCharacterId(),
                        CharacterPersonaFormFields.GetCharacterName(),
                    }
                }
            }
        };

        public static BaseFormSectionDto GetCharacterSelectSection(List<BaseFormDropdownOptionDto> characters) => new BaseFormSectionDto
        {
            SectionTitle = "Select Character",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterPersonaFormFields.GetSelectCharacter(characters),
                    }
                }
            }
        };

        public static BaseFormSectionDto GetPersonaSection() => new BaseFormSectionDto
        {
            SectionTitle = "Persona",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterPersonaFormFields.GetPersonaName(),
                        CharacterPersonaFormFields.GetAttitude(),
                        CharacterPersonaFormFields.GetResponses()
                    }
                }
            }
        };

        public static BaseFormSectionDto GetPlatformsSection() => new BaseFormSectionDto
        {
            SectionTitle = "Platforms",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterPersonaFormFields.GetTwitterPlatformId(),
                        CharacterPersonaFormFields.GetTwitterPlatformName(),
                        CharacterPersonaFormFields.GetTwitterPersonaPlatformId(),

                        CharacterPersonaFormFields.GetFacebookPlatformId(),
                        CharacterPersonaFormFields.GetFacebookPlatformName(),
                        CharacterPersonaFormFields.GetFacebookPersonaPlatformId(),

                        CharacterPersonaFormFields.GetInstagramPlatformId(),
                        CharacterPersonaFormFields.GetInstagramPlatformName(),
                        CharacterPersonaFormFields.GetInstagramPersonaPlatformId(),

                        CharacterPersonaFormFields.GetDiscordPlatformId(),
                        CharacterPersonaFormFields.GetDiscordPlatformName(),
                        CharacterPersonaFormFields.GetDiscordPersonaPlatformId(),

                        CharacterPersonaFormFields.GetTelegramPlatformId(),
                        CharacterPersonaFormFields.GetTelegramPlatformName(),
                        CharacterPersonaFormFields.GetTelegramPersonaPlatformId(),

                    }
                }
            }
        };
    }
}