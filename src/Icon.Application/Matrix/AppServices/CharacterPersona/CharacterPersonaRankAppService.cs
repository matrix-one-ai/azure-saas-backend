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
using Abp.UI;
using Abp.Runtime.Caching;

namespace Icon.Matrix.CharacterPersonas
{
    [AbpAuthorize]
    public class CharacterPersonaRankAppService : IconAppServiceBase
    {

        private readonly IRepository<CharacterPersona, Guid> _characterpersonaRepository;
        private readonly IRepository<CharacterPersonaTwitterRank, Guid> _cpTwitterRankRepository;
        private readonly ISharedSqlRepository<CharacterPersonaTwitterRank> _cpTwitterRankSqlRepository;
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly ICacheManager _cacheManager;

        public CharacterPersonaRankAppService(
            IRepository<CharacterPersona, Guid> characterpersonaRepository,
            IRepository<CharacterPersonaTwitterRank, Guid> cpTwitterRankRepository,
            ISharedSqlRepository<CharacterPersonaTwitterRank> cpTwitterRankSqlRepository,
            IRepository<Character, Guid> characterRepository,
            ICacheManager cacheManager)
        {
            _characterpersonaRepository = characterpersonaRepository;
            _cpTwitterRankRepository = cpTwitterRankRepository;
            _cpTwitterRankSqlRepository = cpTwitterRankSqlRepository;
            _characterRepository = characterRepository;
            _cacheManager = cacheManager;
        }

        [HttpPost]
        public async Task<PagedResultDto<CharacterPersonaRankListDto>> GetCharacterPersonaRanking(GetCharacterPersonasInput input)
        {
            if (input.CharacterName == null)
            {
                throw new UserFriendlyException("Character name is required");
            }

            Guid characterId = Guid.Empty;
            if (input.CharacterName.Equals("plant", StringComparison.OrdinalIgnoreCase))
            {
                characterId = new Guid("77F38589-0DA9-4435-651C-08DD13B3124C"); // plant
            }
            else
            {
                characterId = await _characterRepository
                    .GetAll()
                    .AsNoTracking()
                    .Where(x => x.Name == input.CharacterName)
                    .Select(x => x.Id)
                    .FirstOrDefaultAsync();

                if (characterId == Guid.Empty)
                {
                    throw new UserFriendlyException("Character not found");
                }
            }

            var cacheKey = $"CharacterPersona_{characterId}";
            var myCache = _cacheManager.GetCache<string, List<CharacterPersonaRankListDto>>("CharacterPersonaRanking");
            var mappedCharacterPersonas = await myCache.GetOrDefaultAsync(cacheKey);

            var totalCount = 0;
            var filteredCount = 0;
            if (mappedCharacterPersonas == null || input.ForceRefresh)
            {
                var validUntil = DateTimeOffset.Now.AddMinutes(15);

                var query = GetCharacterPersonasQuery(characterId, withProperties: true).AsNoTracking();
                var allCharacterPersonas = await query.ToListAsync();

                totalCount = allCharacterPersonas.Count;
                mappedCharacterPersonas = MapCharacterPersonas(allCharacterPersonas);

                await myCache.SetAsync(
                    key: cacheKey,
                    value: mappedCharacterPersonas,
                    absoluteExpireTime: validUntil
                );
            }
            else
            {
                totalCount = mappedCharacterPersonas.Count;
            }

            var filteredList = mappedCharacterPersonas
                .WhereIf(!input.PersonaName.IsNullOrEmpty(), cp => (
                    cp.Persona != null) &&
                    (cp.Persona.Name != null && cp.Persona.Name.IndexOf(input.PersonaName, StringComparison.OrdinalIgnoreCase) >= 0
                     || cp.Persona.TwitterHandle != null && cp.Persona.TwitterHandle.IndexOf(input.PersonaName, StringComparison.OrdinalIgnoreCase) >= 0))
                .ToList();

            filteredCount = filteredList.Count;
            if (filteredCount == 0 || filteredCount < input.SkipCount)
            {
                return new PagedResultDto<CharacterPersonaRankListDto>
                {
                    TotalCount = 0,
                    Items = new List<CharacterPersonaRankListDto>()
                };
            }
            else if (filteredCount < totalCount)
            {
                totalCount = filteredCount;
            }

            if (!string.IsNullOrEmpty(input.Sorting))
            {
                filteredList = filteredList
                    .AsQueryable()
                    .OrderBy(input.Sorting)
                    .ToList();
            }

            var filteredPagedList = filteredList
                .Skip(input.SkipCount)
                .Take(input.MaxResultCount)
                .ToList();

            return new PagedResultDto<CharacterPersonaRankListDto>
            {
                TotalCount = totalCount,
                Items = filteredPagedList
            };
        }

        private IQueryable<CharacterPersona> GetCharacterPersonasQuery(Guid characterId, bool withProperties = false)
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

            query = query.Where(x => x.CharacterId == characterId && x.TwitterRank != null && x.TwitterBlockInRanking == false);

            return query;
        }

        private List<CharacterPersonaRankListDto> MapCharacterPersonas(List<CharacterPersona> cps)
        {
            var scores = cps.Select(cp => cp.TwitterRank.TotalScoreTimeDecayed).ToList();
            var sortedScores = scores.OrderByDescending(x => x).ToList();
            var totalGardeners = cps.Count;

            // Define exponential level ratios (smallest to largest group)
            var levelRatios = new double[] { 0.001, 0.01, 0.03, 0.1, 0.859 };
            var levelThresholds = new List<int>();

            // Calculate cumulative thresholds for each level
            int cumulativeSum = 0;
            foreach (var ratio in levelRatios)
            {
                cumulativeSum += (int)(totalGardeners * ratio);
                levelThresholds.Add(cumulativeSum);
            }

            return cps.Select(cp => new CharacterPersonaRankListDto
            {
                Persona = new PersonaTwitterDto
                {
                    Id = cp.Persona.Id,
                    Name = cp.Persona.Name,
                    TwitterHandle = cp.Persona.Platforms?.FirstOrDefault(x => x.Platform?.Name == "Twitter")?.PlatformPersonaId,
                    TwitterAvatarUrl = cp.TwitterProfile?.Avatar
                },
                TwitterRank = ObjectMapper.Map<CharacterPersonaTwitterRankDto>(cp.TwitterRank),
                GardnerLevel = CalculateGardnerLevel(sortedScores.IndexOf(cp.TwitterRank.TotalScoreTimeDecayed) + 1, levelThresholds) // +1 to make index 1-based
            }).ToList();
        }

        private int CalculateGardnerLevel(int scoreRank, List<int> thresholds)
        {
            // Determine the Gardner Level based on thresholds
            if (scoreRank <= thresholds[0]) return 5; // Top 0.1%
            if (scoreRank <= thresholds[1]) return 4; // Top 1%
            if (scoreRank <= thresholds[2]) return 3; // Top 3%
            if (scoreRank <= thresholds[3]) return 2; // Top 10%
            return 1; // Remaining gardeners
        }

        // private int CalculateGardnerLevelOnScore(double totalScore, double minScore, double maxScore)
        // {
        //     var scorePercentage = (totalScore - minScore) / (maxScore - minScore) * 100;

        //     if (scorePercentage > 80) return 5; // Top 20%
        //     if (scorePercentage > 60) return 4; // Top 40%
        //     if (scorePercentage > 40) return 3; // Top 60%
        //     if (scorePercentage > 20) return 2; // Top 80%

        //     return 1; // Bottom 20%
        // }

        // private int CalculateGardnerLevelOnRank(int totalGardeners, int rank)
        // {
        //     var rankPercentage = (totalGardeners - rank) / totalGardeners * 100;

        //     if (rankPercentage > 60) return 5; // Top 20%
        //     if (rankPercentage > 50) return 4; // Top 40%
        //     if (rankPercentage > 40) return 3; // Top 60%
        //     if (rankPercentage > 10) return 2; // Top 80%

        //     return 1; // Bottom 20%
        // }A



    }
}