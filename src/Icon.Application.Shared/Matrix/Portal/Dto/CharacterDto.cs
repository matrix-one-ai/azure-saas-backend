using System;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
namespace Icon.Matrix.Portal.Dto
{
    public class CharacterDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public CharacterBioDto Bio { get; set; }
        public IList<CharacterPersonaDto> Personas { get; set; }
        public string TwitterPostAgentId { get; set; }
        public string TwitterScrapeAgentId { get; set; }
        public bool IsTwitterScrapingEnabled { get; set; }
        public bool IsTwitterPostingEnabled { get; set; }
        public bool IsPromptingEnabled { get; set; }
        public string TwitterUserName { get; set; }
    }

    public class CharacterBioDto : EntityDto<Guid>
    {
        public string Bio { get; set; }
        public string Personality { get; set; }
        public string Appearance { get; set; }
        public string Occupation { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveTo { get; set; }
        public bool IsActive { get; set; }
    }

    // public class CharacterPersonaDto : EntityDto<Guid>
    // {
    //     public PersonaDto Persona { get; set; }
    //     public string Attitude { get; set; }
    //     public string Repsonses { get; set; }
    //     public bool RespondNewPosts { get; set; }
    //     public bool RespondReplies { get; set; }
    //     public bool PersonaIsAi { get; set; }
    // }



    // public class PlatformDto : EntityDto<Guid>
    // {
    //     public string Name { get; set; }
    // }


}