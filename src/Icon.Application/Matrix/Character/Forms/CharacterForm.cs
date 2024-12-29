// aspnet-core/src/Icon.Application/Matrix/Characters/Forms/CharacterForm.cs

using System.Collections.Generic;
using Icon.BaseManagement;

namespace Icon.Matrix.Characters.Forms
{
    public static partial class CharacterForm
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
                        CharacterFormFields.GetCharacterId(),
                    }
                }
            },
            IsHidden = true
        };

        public static BaseFormSectionDto GetMainSection() => new BaseFormSectionDto
        {
            SectionTitle = "Main",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetCharacterName(),
                    }
                }
            }
        };

        public static BaseFormSectionDto GetTwitterSection() => new BaseFormSectionDto
        {
            SectionTitle = "Twitter Integration",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetTwitterUserName(),
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetTwitterScrapeAgentId(),
                        CharacterFormFields.GetTwitterPostAgentId(),
                        CharacterFormFields.GetTwitterPostCommunicationType()
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetIsTwitterScrapingEnabled(),
                        CharacterFormFields.GetIsTwitterPostingEnabled(),
                        CharacterFormFields.GetIsPromptingEnabled()
                    }
                }
            }
        };

        public static BaseFormSectionDto GetTwitterMentionReplyPromptingSection() => new BaseFormSectionDto
        {
            SectionTitle = "Twitter Mention Reply - Prompt instructions",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetTwitterMentionReplyInstruction(),
                        CharacterFormFields.GetTwitterMentionReplyExamples()
                    }
                }
            }
        };

        public static BaseFormSectionDto GetTwitterAutoPostPromptingSection() => new BaseFormSectionDto
        {
            SectionTitle = "Twitter Auto Post - Prompt instructions",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetTwitterAutoPostInstruction(),
                        CharacterFormFields.GetTwitterAutoPostExamples()
                    }
                }
            }
        };

        public static BaseFormSectionDto GetBioSection() => new BaseFormSectionDto
        {
            SectionTitle = "Character Bio",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetBio(),
                        CharacterFormFields.GetIsActive()
                    }
                },
                // new BaseFormRowDto
                // {
                //     Fields = new List<BaseFormFieldDto>
                //     {
                //         CharacterFormFields.GetActiveFrom(),
                //         CharacterFormFields.GetActiveTo(),
                //     }
                // },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetPersonality(),
                        CharacterFormFields.GetAppearance()
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetOccupation(),
                        //CharacterFormFields.GetBirthDate(),
                    }
                },

                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetMotivations(),
                        CharacterFormFields.GetFears(),
                        CharacterFormFields.GetValues()
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetSpeechPatterns(),
                        CharacterFormFields.GetSkills(),
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetBackstory(),
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetPublicPersona(),
                        CharacterFormFields.GetPrivateSelf(),
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetMediaPresence(),
                        CharacterFormFields.GetCrisisBehavior()
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        CharacterFormFields.GetRelationships(),
                        CharacterFormFields.GetTechDetails()
                    }
                }
            }
        };

        /// <summary>
        /// A convenience method to build the entire form if desired.
        /// </summary>
        public static List<BaseFormSectionDto> BuildCharacterForm()
        {
            return new List<BaseFormSectionDto>
            {
                GetMainSection(),
                GetTwitterSection(),
                GetTwitterMentionReplyPromptingSection(),
                GetTwitterAutoPostPromptingSection(),
                GetBioSection()
            };
        }
    }
}
