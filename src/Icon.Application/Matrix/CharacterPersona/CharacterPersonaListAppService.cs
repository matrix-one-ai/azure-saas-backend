

using System.Collections.Generic;
using Abp.Domain.Repositories;
using Icon.Chat.Dto;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Linq.Extensions;
using Abp.RealTime;
using Abp.Runtime.Session;
using Abp.Timing;
using Microsoft.EntityFrameworkCore;
using Icon.Matrix;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Icon.Matrix.Portal.Dto;
using Abp.Collections.Extensions;
using Abp.Localization;
using Abp.UI;
using Icon.Matrix.Models;


namespace Icon.Matrix.CharacterPersonas
{
    [AbpAuthorize]
    public partial class CharacterPersonaAppService : IconAppServiceBase
    {
        private readonly IRepository<CharacterPersona, Guid> _characterpersonaRepository;
        private readonly IRepository<Platform, Guid> _platformRepository;
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly ICharacterManager _characterManager;

        public CharacterPersonaAppService(
            IRepository<CharacterPersona, Guid> characterpersonaRepository,
            IRepository<Platform, Guid> platformRepository,
            IRepository<Character, Guid> characterRepository,
            ICharacterManager characterManager)
        {
            _characterpersonaRepository = characterpersonaRepository;
            _platformRepository = platformRepository;
            _characterRepository = characterRepository;
            _characterManager = characterManager;
        }

        [HttpPost]
        public async Task<PagedResultDto<CharacterPersonaListDto>> GetCharacterPersonas(GetCharacterPersonasInput input)
        {
            var query = GetCharacterPersonasQuery(withProperties: false);
            var filteredQuery = ApplyFiltering(query, input);

            var queryCount = await GetCharacterPersonaCount(filteredQuery);
            query = GetCharacterPersonasQuery(withProperties: true);
            filteredQuery = ApplyFiltering(query, input);

            var characterPersonas = await filteredQuery
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var mappedResult = MapCharacterPersonas(characterPersonas);
            mappedResult = ApplyRowSettings(mappedResult);

            return new PagedResultDto<CharacterPersonaListDto>
            {
                TotalCount = queryCount,
                Items = mappedResult
            };
        }

        private IQueryable<CharacterPersona> GetCharacterPersonasQuery(bool withProperties)
        {
            var query = _characterpersonaRepository.GetAll().AsNoTracking();

            if (withProperties)
            {
                query = query
                    .Include(x => x.TwitterRank)
                    .Include(x => x.Character)
                    .Include(x => x.Persona)
                    .ThenInclude(x => x.Platforms)
                    .ThenInclude(x => x.Platform);
            }

            return query;
        }

        private async Task<int> GetCharacterPersonaCount(IQueryable<CharacterPersona> query)
        {
            var queryCount = await query

            .CountAsync();
            return queryCount;
        }

        private IQueryable<CharacterPersona> ApplyFiltering(IQueryable<CharacterPersona> query, GetCharacterPersonasInput input)
        {
            //throw new UserFriendlyException(input.Sorting);
            query = query
                .WhereIf(input.Sorting.Contains("TwitterRank.Rank"), x => x.TwitterRank != null)
                .WhereIf(!input.CharacterName.IsNullOrEmpty(),
                    x => x.Character.Name.Contains(input.CharacterName))
                .WhereIf(!input.PersonaName.IsNullOrEmpty(),
                    x => x.Persona.Name.Contains(input.PersonaName));


            // query = query
            //     .WhereIf(!input.CharacterPersonaCharacterPersonaFilter.IsNullOrEmpty(),
            //         x => x.CharacterCharacterPersona.CharacterPersona.Name.Contains(input.CharacterPersonaCharacterPersonaFilter))
            //     .WhereIf(!input.CharacterPersonaContentFilter.IsNullOrEmpty(),
            //         x => x.CharacterPersonaContent.Contains(input.CharacterPersonaContentFilter) || x.CharacterPersonaTitle.Contains(input.CharacterPersonaContentFilter))
            //     .WhereIf(input.DateTimeStart.HasValue && input.DateTimeEnd.HasValue,
            //         x => x.CreatedAt >= input.DateTimeStart && x.CreatedAt <= input.DateTimeEnd);

            return query;
        }

        private List<CharacterPersonaListDto> MapCharacterPersonas(List<CharacterPersona> characterPersonas)
        {
            return characterPersonas.Select(characterPersona => new CharacterPersonaListDto
            {
                Id = characterPersona.Id,
                Character = characterPersona.Character == null ? null : new CharacterDto
                {
                    Id = characterPersona.Character.Id,
                    Name = characterPersona.Character.Name
                },
                Persona = characterPersona.Persona == null ? null : new PersonaDto
                {
                    Id = characterPersona.Persona.Id,
                    Name = characterPersona.Persona.Name,

                    PlatformNames = characterPersona.Persona.Platforms == null
                        ? string.Empty
                        : string.Join(", ",
                            characterPersona.Persona.Platforms
                                .Where(p => !string.IsNullOrEmpty(p.PlatformPersonaId))
                                .Select(p => p.Platform.Name)
                                .Where(name => !string.IsNullOrEmpty(name))
                                .Distinct())
                },
                Attitude = characterPersona.Attitude,
                ShouldImportNewPosts = characterPersona.ShouldImportNewPosts,
                ShouldRespondMentions = characterPersona.ShouldRespondMentions,
                ShouldRespondNewPosts = characterPersona.ShouldRespondNewPosts,
                TwitterRank = characterPersona.TwitterRank == null ? null : characterPersona.TwitterRank.Rank,
                RowSettings = new BaseManagement.BaseListRowSettingsDto
                {
                    CanOpen = true,
                    CanEdit = true,
                }
            }).ToList();
        }


        private List<CharacterPersonaListDto> ApplyRowSettings(List<CharacterPersonaListDto> memories)
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