using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Icon.Matrix.AIManager;
using Icon.Matrix.AIManager.CharacterMentioned;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Icon.Matrix.Twitter;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Icon.Matrix
{
    public partial interface IMemoryManager : IDomainService
    {
        Task<Memory> GetMemory(Guid memoryId);
        Task<Memory> UpdateMemory(Memory memory);
        Task DeleteMemory(Guid memoryId);
        Task<List<MemoryType>> GetMemoryTypes();

    }
    public partial class MemoryManager : IconServiceBase, IMemoryManager
    {
        private readonly IRepository<Memory, Guid> _memoryRepository;
        private readonly IRepository<MemoryType, Guid> _memoryTypeRepository;
        private readonly IRepository<MemoryPrompt, Guid> _memoryPromptRepository;
        private readonly IRepository<CharacterPersonaTwitterRank, Guid> _cpTwitterRankRepository;
        private readonly IRepository<MemoryProcess, Guid> _memoryProcessRepository;
        private readonly IRepository<MemoryProcessStep, Guid> _memoryProcessStepRepository;
        private readonly IRepository<MemoryProcessLog, Guid> _memoryProcessLogRepository;

        private readonly IAIManager _aiManager;
        private readonly ITwitterManager _twitterManager;
        private readonly ICharacterManager _characterManager;
        private readonly IPlatformManager _platformManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly int _tenantId;
        private readonly long _userId;


        public MemoryManager(
            IRepository<Memory, Guid> memoryRepository,
            IRepository<MemoryType, Guid> memoryTypeRepository,
            IRepository<MemoryPrompt, Guid> memoryPromptRepository,

            IRepository<CharacterPersonaTwitterRank, Guid> cpTwitterRankRepository,
            IRepository<MemoryProcess, Guid> memoryProcessRepository,
            IRepository<MemoryProcessStep, Guid> memoryProcessStepRepository,
            IRepository<MemoryProcessLog, Guid> memoryProcessLogRepository,

            IAIManager aiManager,
            ITwitterManager twitterManager,
            ICharacterManager characterManager,
            IPlatformManager platformManager,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession)
        {
            _memoryRepository = memoryRepository;
            _memoryTypeRepository = memoryTypeRepository;
            _memoryPromptRepository = memoryPromptRepository;
            _cpTwitterRankRepository = cpTwitterRankRepository;
            _memoryProcessRepository = memoryProcessRepository;
            _memoryProcessStepRepository = memoryProcessStepRepository;
            _memoryProcessLogRepository = memoryProcessLogRepository;

            _aiManager = aiManager;
            _twitterManager = twitterManager;
            _characterManager = characterManager;
            _platformManager = platformManager;
            _unitOfWorkManager = unitOfWorkManager;


            if (abpSession.TenantId == null || abpSession.UserId == null)
            {
                throw new UserFriendlyException("TenantId or UserId is null");
            }

            _tenantId = abpSession.TenantId.Value;
            _userId = abpSession.UserId.Value;
        }

        public async Task<Memory> GetMemory(Guid memoryId)
        {
            var memory = await _memoryRepository.GetAll()
                .Include(x => x.Character)
                .Include(x => x.CharacterBio)
                .Include(x => x.MemoryType)
                .Include(x => x.Prompts)
                .Include(x => x.Platform)

                .Include(x => x.CharacterPersona)
                    .ThenInclude(cp => cp.Persona)
                    .ThenInclude(p => p.Platforms)
                    .ThenInclude(p => p.Platform)
                .Include(x => x.CharacterPersona)
                    .ThenInclude(cp => cp.TwitterRank)

                .Include(x => x.MemoryStatsTwitter)

                .Include(x => x.MemoryProcess)
                    .ThenInclude(x => x.Steps)
                .Include(x => x.MemoryProcess)
                    .ThenInclude(x => x.Logs)

                .Where(x => x.Id == memoryId)
                .FirstOrDefaultAsync();

            return memory;
        }

        public async Task<Memory> CreateMemory(Memory memory)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                memory = await _memoryRepository.InsertAsync(memory);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }

            // exclude A4C40AD1-83E2-489A-E4D8-08DD13B31247
            var excludedCharacters = new List<Guid> { Guid.Parse("A4C40AD1-83E2-489A-E4D8-08DD13B31247") }; // sami
            if (!excludedCharacters.Contains(memory.CharacterId))
            {
                await CreateMemoryProcess(memory.Id);
            }

            return memory;
        }

        public async Task<Memory> UpdateMemory(Memory memory)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                memory = await _memoryRepository.UpdateAsync(memory);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }

            return memory;
        }

        public async Task DeleteMemory(Guid memoryId)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _memoryRepository.DeleteAsync(memoryId);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }
        }


        public async Task<Guid> GetMemoryTypeId(string name)
        {
            var memoryType = await GetMemoryType(name);
            return memoryType.Id;
        }

        public async Task<MemoryType> GetMemoryType(string name)
        {
            var memoryType = await _memoryTypeRepository.FirstOrDefaultAsync(m => m.Name == name);
            if (memoryType == null)
            {
                memoryType = await CreateMemoryType(name);
            }
            return memoryType;
        }

        public async Task<MemoryType> CreateMemoryType(string name)
        {
            var memoryType = new MemoryType
            {
                TenantId = _tenantId,
                Name = name
            };

            using (var uow = _unitOfWorkManager.Begin())
            {
                memoryType = await _memoryTypeRepository.InsertAsync(memoryType);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }

            return memoryType;
        }

        public async Task<List<MemoryType>> GetMemoryTypes()
        {
            var memoryTypes = await _memoryTypeRepository.GetAllListAsync();
            return memoryTypes;
        }



    }
}