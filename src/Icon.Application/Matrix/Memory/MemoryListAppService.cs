using System.Collections.Generic;
using Abp.Domain.Repositories;
using Icon.Chat.Dto;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Abp.Collections.Extensions;
using Abp.Linq.Extensions;
using System.Linq.Dynamic.Core;
using System;
using Icon.AzureStorage;
using Icon.Matrix.Portal.Dto;
using Icon.Matrix.Models;
using Icon.Matrix.AIManager;
using Icon.Matrix.Twitter;

namespace Icon.Matrix.Memories
{
    [AbpAuthorize]
    public partial class MemoryAppService : IconAppServiceBase
    {
        private readonly IMemoryManager _memoryManager;
        private readonly IAIManager _aiManager;
        private readonly IRepository<Memory, Guid> _memoryRepository;
        private readonly IRepository<CharacterPersonaTwitterRank, Guid> _cpTwitterRankRepository;
        private readonly ITwitterCommunicationService _twitterCommunicationService;

        public MemoryAppService(
            IMemoryManager memoryManager,
            IAIManager aiManager,
            IRepository<Memory, Guid> memoryRepository,
            IRepository<CharacterPersonaTwitterRank, Guid> cpTwitterRankRepository,
            ITwitterCommunicationService twitterCommunicationService)
        {
            _memoryManager = memoryManager;
            _aiManager = aiManager;
            _memoryRepository = memoryRepository;
            _cpTwitterRankRepository = cpTwitterRankRepository;
            _twitterCommunicationService = twitterCommunicationService;
        }

        // public async Task<MemoryDto> StoreMemory(StoreMemoryInput input)
        // {
        //     var memory = await _memoryManager.StoreMemory(
        //         input.CharacterName,
        //         input.MemoryTitle,
        //         input.MemoryContent,
        //         input.MemoryType,
        //         input.PlatformName,
        //         input.PlatformPersonaId,
        //         input.PlatformPersonaName,
        //         input.PlatformInteractionId,
        //         input.PlatformInteractionParentId,
        //         input.PlatformInteractionDate
        //     );

        //     return ObjectMapper.Map<MemoryDto>(memory);
        // }

        public bool ShouldReceiveMemories()
        {
            return true;
        }


        [HttpPost]
        public async Task<PagedResultDto<MemoryListDto>> GetMemories(GetMemoriesInput input)
        {
            var query = GetMemoriesQuery(withProperties: true);
            var filteredQuery = ApplyFiltering(query, input);

            var queryCount = await GetMemoryCount(filteredQuery);
            var memories = await filteredQuery
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var mappedResult = ObjectMapper.Map<List<MemoryListDto>>(memories);
            mappedResult = ApplyRowSettings(mappedResult);

            return new PagedResultDto<MemoryListDto>
            {
                TotalCount = queryCount,
                Items = mappedResult
            };
        }

        private IQueryable<Memory> GetMemoriesQuery(bool withProperties)
        {
            var query = _memoryRepository.GetAll().AsNoTracking();

            if (withProperties)
            {
                query = query
                    .Include(x => x.Character)
                    .Include(x => x.Platform)
                    .Include(x => x.MemoryType)
                    .Include(x => x.CharacterPersona)
                        .ThenInclude(cp => cp.Persona);
            }

            return query;
        }

        private async Task<int> GetMemoryCount(IQueryable<Memory> query)
        {
            var queryCount = await query.CountAsync();
            return queryCount;
        }

        private IQueryable<Memory> ApplyFiltering(IQueryable<Memory> query, GetMemoriesInput input)
        {
            query = query
                .WhereIf(!input.MemoryCharacter.IsNullOrEmpty(),
                    x => x.Character.Name.Contains(input.MemoryCharacter))
                .WhereIf(!input.MemoryPersona.IsNullOrEmpty(),
                    x => x.CharacterPersona.Persona.Name.Contains(input.MemoryPersona))
                .WhereIf(input.MemoryTypeId.HasValue,
                    x => x.MemoryTypeId == input.MemoryTypeId)
                .WhereIf(!input.MemoryContent.IsNullOrEmpty(),
                    x => x.MemoryContent.Contains(input.MemoryContent) || x.MemoryTitle.Contains(input.MemoryContent))
                .WhereIf(input.DateTimeStart.HasValue && input.DateTimeEnd.HasValue,
                    x => x.CreatedAt >= input.DateTimeStart && x.CreatedAt <= input.DateTimeEnd);

            return query;
        }

        private List<MemoryListDto> ApplyRowSettings(List<MemoryListDto> memories)
        {
            foreach (var memory in memories)
            {
                if (memory.MemoryType.Name != "CharacterMentionedTweet")
                {
                    memory.IsActionTaken = null;
                    memory.IsPromptGenerated = null;
                }

                memory.RowSettings = new BaseManagement.BaseListRowSettingsDto
                {
                    CanOpen = true,
                    CanEdit = true,
                };
            }

            return memories;
        }

    }
}