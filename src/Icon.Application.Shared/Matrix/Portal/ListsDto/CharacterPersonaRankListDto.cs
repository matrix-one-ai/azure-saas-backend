using System;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;

namespace Icon.Matrix.Portal.Dto
{
    public class CharacterPersonaRankListDto
    {
        public Guid Id { get; set; }
        public CharacterSimpleDto Character { get; set; }
        public PersonaSimpleDto Persona { get; set; }
        public string Attitude { get; set; }
        public string Responses { get; set; }
        public bool ShouldRespondNewPosts { get; set; }
        public bool ShouldRespondMentions { get; set; }
        public bool ShouldImportNewPosts { get; set; }
        public CharacterPersonaTwitterRankDto TwitterRank { get; set; }
        public BaseListRowSettingsDto RowSettings { get; set; }

    }
}