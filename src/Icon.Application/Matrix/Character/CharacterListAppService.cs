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

namespace Icon.Matrix
{
    [AbpAuthorize]
    public partial class CharacterAppService : IconAppServiceBase
    {
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly IRepository<CharacterBio, Guid> _characterBioRepository;
        private readonly IRepository<CharacterPlatform, Guid> _characterPlatformRepository;
        private readonly IRepository<CharacterTopic, Guid> _characterTopicRepository;
        private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;

        public CharacterAppService(
            IRepository<Character, Guid> characterRepository,
            IRepository<CharacterBio, Guid> characterBioRepository,
            IRepository<CharacterPlatform, Guid> characterPlatformRepository,
            IRepository<CharacterTopic, Guid> characterTopicRepository,
            IRepository<CharacterPersona, Guid> characterPersonaRepository)
        {
            _characterRepository = characterRepository;
            _characterBioRepository = characterBioRepository;
            _characterPlatformRepository = characterPlatformRepository;
            _characterTopicRepository = characterTopicRepository;
            _characterPersonaRepository = characterPersonaRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<CharacterListDto>> GetCharacters(GetCharactersInput input)
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

            var mappedResult = ObjectMapper.Map<List<CharacterListDto>>(characters);
            mappedResult = ApplyRowSettings(mappedResult);

            return new PagedResultDto<CharacterListDto>
            {
                TotalCount = queryCount,
                Items = mappedResult
            };
        }

        private IQueryable<Character> GetCharactersQuery()
        {
            var query = _characterRepository.GetAll().AsNoTracking();
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
            var baseQuery = _characterRepository.GetAll().AsNoTracking();

            // if (input.IncludePersonas)
            // {
            //     baseQuery = baseQuery.Include(p => p.Personas)
            //         .ThenInclude(p => p.Persona)
            //         .ThenInclude(p => p.Platforms)
            //         .ThenInclude(p => p.Platform);

            //     if (!string.IsNullOrEmpty(input.PlatformName))
            //     {
            //         baseQuery = baseQuery.Select(c => new Character
            //         {
            //             Id = c.Id,
            //             Name = c.Name,
            //             Personas = c.Personas
            //                 .Where(p => p.Persona.Platforms.Any(pl => pl.Platform.Name == input.PlatformName))
            //                 .ToList(),
            //             Bios = input.IncludeCharacterBios ? c.Bios : null
            //         });
            //     }
            // }

            if (input.IncludeCharacterBios)
            {
                baseQuery = baseQuery.Include(p => p.Bios);
            }

            return baseQuery;
        }

        private IQueryable<Character> ApplyFiltering(IQueryable<Character> query, GetCharactersInput input)
        {
            query = query
                .WhereIf(!input.CharacterName.IsNullOrEmpty(), x => x.Name.Contains(input.CharacterName));

            return query;
        }

        private List<CharacterListDto> ApplyRowSettings(List<CharacterListDto> memories)
        {
            foreach (var characterpersona in memories)
            {
                characterpersona.RowSettings = new BaseManagement.BaseListRowSettingsDto
                {
                    CanOpen = true,
                    CanEdit = true,
                };
            }

            return memories;
        }
    }
}