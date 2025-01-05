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
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Icon.Matrix.Portal.Dto;
using Abp.Collections.Extensions;
using Icon.EntityFrameworkCore.Matrix;
using Icon.Matrix.Models;

namespace Icon.Matrix.CharacterPersonas
{
    [AbpAuthorize]
    public class CharacterPersonaRankAppService : IconAppServiceBase
    {

        private readonly IRepository<CharacterPersona, Guid> _characterpersonaRepository;
        private readonly IRepository<CharacterPersonaTwitterRank, Guid> _cpTwitterRankRepository;
        private readonly ISharedSqlRepository<CharacterPersonaTwitterRank> _cpTwitterRankSqlRepository;

        public CharacterPersonaRankAppService(
            IRepository<CharacterPersona, Guid> characterpersonaRepository,
            IRepository<CharacterPersonaTwitterRank, Guid> cpTwitterRankRepository,
            ISharedSqlRepository<CharacterPersonaTwitterRank> cpTwitterRankSqlRepository)
        {
            _characterpersonaRepository = characterpersonaRepository;
            _cpTwitterRankRepository = cpTwitterRankRepository;
            _cpTwitterRankSqlRepository = cpTwitterRankSqlRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<CharacterPersonaRankListDto>> GetCharacterPersonaRanking(GetCharacterPersonasInput input)
        {
            var query = GetCharacterPersonasQuery(withProperties: false);
            var filteredQuery = ApplyFiltering(query, input);

            var queryCount = await GetCharacterPersonaCount(filteredQuery);
            query = GetCharacterPersonasQuery(withProperties: true);
            filteredQuery = ApplyFiltering(query, input);

            var characterPersonas = await filteredQuery
                .OrderBy("TwitterRank.Rank")
                .PageBy(input)
                .ToListAsync();

            var mappedResult = MapCharacterPersonas(characterPersonas);
            mappedResult = ApplyRowSettings(mappedResult);

            return new PagedResultDto<CharacterPersonaRankListDto>
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
                    .Include(x => x.TwitterProfile)
                    .Include(x => x.Character)
                    .Include(x => x.Persona)
                        .ThenInclude(x => x.Platforms)
                        .ThenInclude(x => x.Platform);
            }

            return query;
        }

        private async Task<int> GetCharacterPersonaCount(IQueryable<CharacterPersona> query)
        {
            var queryCount = await query.CountAsync();
            return queryCount;
        }

        private IQueryable<CharacterPersona> ApplyFiltering(IQueryable<CharacterPersona> query, GetCharacterPersonasInput input)
        {
            query = query
                .Where(x => x.TwitterRank != null)
                .WhereIf(!input.CharacterName.IsNullOrEmpty(),
                    x => x.Character.Name.Contains(input.CharacterName))
                .WhereIf(!input.PersonaName.IsNullOrEmpty(),
                    x => x.Persona.Name.Contains(input.PersonaName));

            return query;
        }

        private List<CharacterPersonaRankListDto> MapCharacterPersonas(List<CharacterPersona> cps)
        {
            return cps.Select(cp => new CharacterPersonaRankListDto
            {
                Id = cp.Id,
                Character = ObjectMapper.Map<CharacterSimpleDto>(cp.Character),
                Persona = new PersonaTwitterDto
                {
                    Id = cp.Persona.Id,
                    Name = cp.Persona.Name,
                    TwitterHandle = cp.Persona.Platforms?.FirstOrDefault(x => x.Platform?.Name == "Twitter")?.PlatformPersonaId,
                    TwitterAvatarUrl = cp.TwitterProfile?.Avatar
                },
                TwitterRank = ObjectMapper.Map<CharacterPersonaTwitterRankDto>(cp.TwitterRank),

                Attitude = cp.Attitude,
                ShouldImportNewPosts = cp.ShouldImportNewPosts,
                ShouldRespondMentions = cp.ShouldRespondMentions,
                ShouldRespondNewPosts = cp.ShouldRespondNewPosts,
                RowSettings = new BaseManagement.BaseListRowSettingsDto
                {
                    CanOpen = true,
                    CanEdit = false,
                }
            }).ToList();
        }

        // private string GetTwitterAvatarUrl(string platformPersonaId)
        // {
        //     if (platformPersonaId.IsNullOrEmpty())
        //     {
        //         return null;
        //     }

        //     // if id starts with @, remove it
        //     var username = platformPersonaId.StartsWith("@") ? platformPersonaId.Substring(1) : platformPersonaId;
        //     var url = $"https://x.com/{username}/photo";

        //     return url;
        // }

        private List<CharacterPersonaRankListDto> ApplyRowSettings(List<CharacterPersonaRankListDto> ranks)
        {
            foreach (var characterpersona in ranks)
            {
                characterpersona.RowSettings = new BaseManagement.BaseListRowSettingsDto
                {
                    CanOpen = true,
                    CanEdit = true,
                };
            }

            return ranks;
        }

    }
}