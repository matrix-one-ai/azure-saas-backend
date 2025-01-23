using System.Collections.Generic;
using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using Abp.UI;
using Icon.Matrix.Portal.Dto;
using Icon.Matrix.Models;
using Microsoft.AspNetCore.Mvc;
using Abp.Collections.Extensions;
using System.Linq.Dynamic.Core;
using Icon.Matrix.AIManager;
using Icon.Matrix.Twitter;
using Icon.Matrix.Eliza;
using Icon.Authorization.Users;

namespace Icon.Matrix.ElizaCharacters
{
    [AbpAuthorize]
    public partial class ElizaCharacterAppService : IconAppServiceBase
    {
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly IRepository<CharacterBio, Guid> _characterBioRepository;
        private readonly IRepository<CharacterPlatform, Guid> _characterPlatformRepository;
        private readonly IRepository<CharacterTopic, Guid> _characterTopicRepository;
        private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;

        private readonly ICharacterManager _characterManager;
        private readonly IAIManager _aiManager;
        private readonly ITwitterManager _twitterManager;
        private readonly IMemoryManager _memoryManager;

        public ElizaCharacterAppService(
            IRepository<Character, Guid> characterRepository,
            IRepository<CharacterBio, Guid> characterBioRepository,
            IRepository<CharacterPlatform, Guid> characterPlatformRepository,
            IRepository<CharacterTopic, Guid> characterTopicRepository,
            IRepository<CharacterPersona, Guid> characterPersonaRepository,
            ICharacterManager characterManager,
            IAIManager aiManager,
            ITwitterManager twitterManager,
            IMemoryManager memoryManager)
        {
            _characterRepository = characterRepository;
            _characterBioRepository = characterBioRepository;
            _characterPlatformRepository = characterPlatformRepository;
            _characterTopicRepository = characterTopicRepository;
            _characterPersonaRepository = characterPersonaRepository;
            _characterManager = characterManager;
            _aiManager = aiManager;
            _twitterManager = twitterManager;
            _memoryManager = memoryManager;
        }

        [HttpPost]
        public async Task<List<ElizaCharacter>> GetCharacters(GetCharactersInput input)
        {
            var query = GetCharactersQuery();
            var filteredQuery = ApplyFiltering(query, input);

            var queryCount = await GetCharactersCount(filteredQuery);
            query = GetCharactersQuery();
            filteredQuery = ApplyFiltering(query, input);

            var characters = await filteredQuery
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var mappedResult = MapToElizaCharacters(characters);

            return mappedResult;
        }

        private IQueryable<Character> GetCharactersQuery()
        {
            var query = _characterRepository.GetAll().Include(c => c.Bios).AsNoTracking();
            return query;
        }

        private async Task<int> GetCharactersCount(IQueryable<Character> query)
        {
            var queryCount = await query.CountAsync();
            return queryCount;
        }

        public async Task<CharacterDto> GetCharacter(GetCharacterInput input)
        {
            // input.CharacterName = "Sami";
            // input.IncludePersonas = true;
            // input.PlatformName = "Twitter";

            if (!input.CharacterId.HasValue && string.IsNullOrWhiteSpace(input.CharacterName))
            {
                throw new UserFriendlyException("Either CharacterId or CharacterName must be provided.");
            }

            var baseQuery = GetCharacterQuery(input);

            var character = await baseQuery
                .WhereIf(input.CharacterId.HasValue, x => x.Id == input.CharacterId)
                .WhereIf(!string.IsNullOrWhiteSpace(input.CharacterName), x => x.Name == input.CharacterName)
                .FirstOrDefaultAsync();

            return ObjectMapper.Map<CharacterDto>(character);
        }

        public async Task<CharacterDto> CreateCharacter(CreateCharacterInput input)
        {
            var character = ObjectMapper.Map<Character>(input);
            character.TenantId = AbpSession.TenantId.Value;
            character = await _characterRepository.InsertAsync(character);

            return ObjectMapper.Map<CharacterDto>(character);
        }

        public async Task<CharacterBioDto> CreateCharacterBio(CreateCharacterBioInput input)
        {
            var characterBio = ObjectMapper.Map<CharacterBio>(input);
            characterBio.TenantId = AbpSession.TenantId.Value;
            characterBio = await _characterBioRepository.InsertAsync(characterBio);

            return ObjectMapper.Map<CharacterBioDto>(characterBio);
        }

        public IQueryable<Character> GetCharacterQuery(GetCharacterInput input)
        {
            var baseQuery = _characterRepository
                .GetAll()
                .Include(c => c.Bios)
                .AsNoTracking();

            return baseQuery;
        }

        private IQueryable<Character> ApplyFiltering(IQueryable<Character> query, GetCharactersInput input)
        {
            query = query
                .WhereIf(!input.CharacterName.IsNullOrEmpty(), x => x.Name.Contains(input.CharacterName));

            return query;
        }

        private List<ElizaCharacter> MapToElizaCharacters(List<Character> characters)
        {
            var elizaCharacters = new List<ElizaCharacter>();

            foreach (var character in characters)
            {
                var elizaCharacter = new ElizaCharacter
                {
                    Id = character.Id.ToString(),
                    Name = character.Name,
                    ModelProvider = "openai", //ElizaModelProviderName.OPENAI,
                    Settings = new ElizaSettings
                    {
                        Voice = new ElizaVoiceSettings
                        {
                            Model = "en_US-male-medium"
                        },
                    },
                    Plugins = new List<ElizaPlugin> { },
                    Bio = new List<string> { character.Bios?.FirstOrDefault()?.Bio },
                    Lore = new List<string> { character.Bios?.FirstOrDefault()?.Values },
                    MessageExamples = new List<List<ElizaMessageExample>>(),
                    PostExamples = new List<string> { character.TwitterAutoPostExamples },
                    Topics = new List<string> { character.Bios?.FirstOrDefault()?.Skills },
                    // Clients = new List<ElizaClients> { ElizaClients.TWITTER, ElizaClients.DISCORD, ElizaClients.TELEGRAM },
                    Style = new ElizaStyle
                    {
                        All = new List<string>(),
                        Chat = new List<string>(),
                        Post = new List<string>(),
                    },
                    Adjectives = new List<string>(),
                };

                elizaCharacters.Add(elizaCharacter);
            }

            return elizaCharacters;
        }


    }
}