using System;
using Abp.Application.Services.Dto;
using Icon.BaseManagement;

namespace Icon.Matrix.Portal.Dto
{
    public class CharacterListDto : EntityDto<Guid>
    {
        public string Name { get; set; }
        public CharacterBioDto Bio { get; set; }
        public string TwitterPostAgentId { get; set; }
        public string TwitterScrapeAgentId { get; set; }
        public bool IsTwitterScrapingEnabled { get; set; }
        public bool IsTwitterPostingEnabled { get; set; }
        public bool IsPromptingEnabled { get; set; }
        public string TwitterUserName { get; set; }
        public BaseListRowSettingsDto RowSettings { get; set; }
    }
}