using System;
using System.Collections.Generic;
using Icon.BaseManagement;
using Icon.Matrix.Portal.Dto;

namespace Icon.Matrix.Memories.Forms
{
    public static class MemoryForm
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
                        MemoryFormFields.GetMemoryId(),
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
                        MemoryFormFields.GetCharacterId(),
                        MemoryFormFields.GetCharacterName(),
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
                        MemoryFormFields.GetPersonaName(),
                    }
                }
            }
        };

        public static BaseFormSectionDto GetMemorySection() => new BaseFormSectionDto
        {
            SectionTitle = "Memory",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        MemoryFormFields.GetPlatformName(),
                        MemoryFormFields.GetMemoryTypeName(),
                    }
                },
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        MemoryFormFields.GetMemoryContent(),
                        // MemoryFormFields.GetMemoryUrl(),
                    }
                }

            }
        };

        public static BaseFormSectionDto GetPromptSection() => new BaseFormSectionDto
        {
            SectionTitle = "Prompt",
            Rows = new List<BaseFormRowDto>
            {
                new BaseFormRowDto
                {
                    Fields = new List<BaseFormFieldDto>
                    {
                        MemoryFormFields.GetMemoryPromptOutput(),
                    }
                }
            }
        };


    }
}