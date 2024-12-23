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
using Icon.Friendships.Cache;
using Icon.Friendships.Dto;
using Icon.Matrix;
using System;
using JetBrains.Annotations;
using Abp.UI;
using Icon.Authorization;
using Icon.Matrix.Twitter.Dto;
using Icon.Matrix.Twitter.Inputs;
using Icon.Matrix.Models;

namespace Icon.Matrix
{
    [AbpAuthorize]
    public class TwitterAppService : IconAppServiceBase
    {
        // private readonly IRepository<Character, Guid> _characterRepository;
        // private readonly IRepository<CharacterBio, Guid> _characterBioRepository;
        // private readonly IRepository<CharacterPlatform, Guid> _characterPlatformRepository;
        // private readonly IRepository<CharacterTopic, Guid> _characterTopicRepository;
        // private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;

        // public TwitterAppService(
        //     IRepository<Character, Guid> characterRepository,
        //     IRepository<CharacterBio, Guid> characterBioRepository,
        //     IRepository<CharacterPlatform, Guid> characterPlatformRepository,
        //     IRepository<CharacterTopic, Guid> characterTopicRepository,
        //     IRepository<CharacterPersona, Guid> characterPersonaRepository)
        // {
        //     _characterRepository = characterRepository;
        //     _characterBioRepository = characterBioRepository;
        //     _characterPlatformRepository = characterPlatformRepository;
        //     _characterTopicRepository = characterTopicRepository;
        //     _characterPersonaRepository = characterPersonaRepository;
        // }

        // public async Task<List<TwitterPersonaDto>> GetCharacterPersonas(GetTwitterCharacterPersonasInput input)
        // {
        //     var charcterInput = new GetCharacterInput
        //     {
        //         CharacterName = input.CharacterName,
        //         IncludePersonas = true,
        //         PlatformName = "Twitter"
        //     };

        //     if (!input.CharacterId.HasValue && string.IsNullOrWhiteSpace(input.CharacterName))
        //     {
        //         throw new UserFriendlyException("Either CharacterId or CharacterName must be provided.");
        //     }

        //     var baseQuery = GetCharacterQuery(charcterInput);

        //     // var limitTo = new List<string> { "@neuralink", "@elonmusk", "@SpaceX", "@Tesla" };

        //     var character = await baseQuery
        //         .WhereIf(input.CharacterId.HasValue, x => x.Id == input.CharacterId)
        //         .WhereIf(!string.IsNullOrWhiteSpace(input.CharacterName), x => x.Name == input.CharacterName)
        //         .FirstOrDefaultAsync();

        //     var personas = character.Personas.Select(p => new TwitterPersonaDto
        //     {
        //         CharacterId = character.Id,
        //         CharacterName = character.Name,
        //         PersonaId = p.Persona.Id,
        //         PlatformPersonaName = p.Persona.Name,
        //         PlatformPersonaId = p.Persona.Platforms.FirstOrDefault(pl => pl.Platform.Name == charcterInput.PlatformName).PlatformPersonaId
        //     }).ToList();

        //     //personas = personas.Where(p => limitTo.Contains(p.PlatformPersonaId)).ToList();

        //     return personas;
        // }

        // public async Task<TwitterCharacterDto> GetCharacter(GetTwitterCharacterInput input)
        // {
        //     var charcterInput = new GetCharacterInput
        //     {
        //         CharacterName = input.CharacterName,
        //         PlatformName = "Twitter"
        //     };

        //     if (!input.CharacterId.HasValue && string.IsNullOrWhiteSpace(input.CharacterName))
        //     {
        //         throw new UserFriendlyException("Either CharacterId or CharacterName must be provided.");
        //     }

        //     var baseQuery = GetCharacterQuery(charcterInput);

        //     var character = await baseQuery
        //         .WhereIf(input.CharacterId.HasValue, x => x.Id == input.CharacterId)
        //         .WhereIf(!string.IsNullOrWhiteSpace(input.CharacterName), x => x.Name == input.CharacterName)
        //         .FirstOrDefaultAsync();

        //     if (character == null)
        //     {
        //         throw new UserFriendlyException("Character not found.");
        //     }

        //     if (character.Platforms.All(p => p.Platform.Name != charcterInput.PlatformName))
        //     {
        //         throw new UserFriendlyException("Character does not have a Twitter platform.");
        //     }

        //     var twitterCharacter = new TwitterCharacterDto
        //     {
        //         CharacterId = character.Id,
        //         CharacterName = character.Name,
        //         PlatformCharacterId = character.Platforms.FirstOrDefault(p => p.Platform.Name == charcterInput.PlatformName).PlatformCharacterId
        //     };

        //     return twitterCharacter;
        // }

        // private IQueryable<Character> GetCharacterQuery(GetCharacterInput input)
        // {
        //     var baseQuery = _characterRepository.GetAll().AsNoTracking();
        //     baseQuery = baseQuery.Include(p => p.Platforms).ThenInclude(p => p.Platform);

        //     if (input.IncludePersonas)
        //     {
        //         baseQuery = baseQuery

        //             .Include(p => p.Personas)
        //             .ThenInclude(p => p.Persona)
        //             .ThenInclude(p => p.Platforms)
        //             .ThenInclude(p => p.Platform);

        //         if (!string.IsNullOrEmpty(input.PlatformName))
        //         {
        //             baseQuery = baseQuery.Select(c => new Character
        //             {
        //                 Id = c.Id,
        //                 Name = c.Name,
        //                 Personas = c.Personas
        //                     .Where(p => p.Persona.Platforms.Any(pl => pl.Platform.Name == input.PlatformName))
        //                     .ToList(),
        //                 Bios = input.IncludeCharacterBios ? c.Bios : null
        //             });
        //         }
        //     }

        //     if (input.IncludeCharacterBios)
        //     {
        //         baseQuery = baseQuery.Include(p => p.Bios);
        //     }

        //     return baseQuery;
        // }
    }
}