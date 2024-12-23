using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities.Auditing;
using Icon.BaseManagement;

namespace Icon.Matrix.Portal.Dto
{
    public class MemoryDto : EntityDto<Guid>
    {
        public CharacterDto Character { get; set; }
        public CharacterBioDto CharacterBio { get; set; }
        public CharacterPersonaDto CharacterPersona { get; set; }

        public PlatformDto Platform { get; set; }
        public string PlatformInteractionId { get; set; }
        public string PlatformInteractionParentId { get; set; }
        public DateTimeOffset? PlatformInteractionDate { get; set; }

        public MemoryTypeDto MemoryType { get; set; }
        public string MemoryTitle { get; set; }
        public string MemoryContent { get; set; }

        public int RememberDays { get; set; }

        public string Tags { get; set; }
        public DateTimeOffset CreatedAt { get; set; }

        public string PromptForAction { get; set; }
        public string PromptResponse { get; set; }
        public ICollection<Action> Actions { get; set; } = new List<Action>();

        public bool ShouldVectorize { get; set; }
        public string VectorHash { get; set; }


    }
}