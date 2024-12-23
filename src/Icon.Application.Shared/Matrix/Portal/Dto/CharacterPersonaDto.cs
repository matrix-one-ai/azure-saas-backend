using System;
using Abp.Application.Services.Dto;

namespace Icon.Matrix.Portal.Dto
{
    public class CharacterPersonaDto : EntityDto<Guid>
    {
        public PersonaDto Persona { get; set; }
        public string Attitude { get; set; }
        public string Responses { get; set; }
        public bool RespondNewPosts { get; set; }
        public bool RespondReplies { get; set; }
        public bool PersonaIsAi { get; set; }
    }
}