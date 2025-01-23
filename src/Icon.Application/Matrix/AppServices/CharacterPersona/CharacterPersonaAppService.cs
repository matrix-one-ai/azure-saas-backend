

using System.Collections.Generic;
using Abp.Domain.Repositories;
using Icon.Chat.Dto;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using Microsoft.EntityFrameworkCore;
using Icon.Matrix;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Dynamic.Core;
using Icon.Matrix.Portal.Dto;
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
        private readonly IRepository<PersonaPlatform, Guid> _personaPlatformRepository;
        private readonly ICharacterManager _characterManager;

        public CharacterPersonaAppService(
            IRepository<CharacterPersona, Guid> characterpersonaRepository,
            IRepository<Platform, Guid> platformRepository,
            IRepository<Character, Guid> characterRepository,
            IRepository<PersonaPlatform, Guid> personaPlatformRepository,
            ICharacterManager characterManager)
        {
            _characterpersonaRepository = characterpersonaRepository;
            _platformRepository = platformRepository;
            _characterRepository = characterRepository;
            _personaPlatformRepository = personaPlatformRepository;
            _characterManager = characterManager;
        }

        [HttpPost]
        public async Task<PersonaTwitterWalletDto> UpdateCharacterPersonaSolanaWallet(UpdatePersonaWalletInput input)
        {
            var character = await _characterRepository.FirstOrDefaultAsync(x => x.Name == input.CharacterName);
            if (character == null)
            {
                throw new UserFriendlyException("Character not found");
            }

            if (input.PersonaName == null || string.IsNullOrEmpty(input.PersonaName))
            {
                input.PersonaName = input.TwitterHandle;
            }

            var twitterPlatform = await _platformRepository.FirstOrDefaultAsync(x => x.Name == "Twitter");
            var characterPersonaId = await _characterManager.GetCharacterPersonaId(character.Id, twitterPlatform.Id, input.TwitterHandle, input.PersonaName);

            var query = GetCharacterPersonaQuery(withProperties: true);
            var characterPersona = await query.FirstOrDefaultAsync(x => x.Id == characterPersonaId);

            if (characterPersona == null)
            {
                throw new UserFriendlyException("Persona not found");
            }

            var personaPlatform = characterPersona.Persona?.Platforms?.FirstOrDefault(p => p.Platform.Name == "Twitter");
            if (personaPlatform == null)
            {
                throw new UserFriendlyException("Twitter platform not found for persona");
            }

            characterPersona.SolanaWallet = input.SolanaWallet;
            await _characterpersonaRepository.UpdateAsync(characterPersona);
            await CurrentUnitOfWork.SaveChangesAsync();

            return new PersonaTwitterWalletDto
            {
                Id = characterPersona.Id,
                Name = characterPersona.Persona.Name,
                TwitterHandle = personaPlatform.PlatformPersonaId,
                TwitterAvatarUrl = characterPersona.TwitterProfile?.Avatar,
                SolanaWallet = characterPersona.SolanaWallet
            };
        }

        private IQueryable<CharacterPersona> GetCharacterPersonaQuery(bool withProperties)
        {
            var query = _characterpersonaRepository.GetAll();

            if (withProperties)
            {
                query = query
                    .Include(x => x.TwitterProfile)
                    .Include(x => x.Character)
                    .Include(x => x.Persona)
                    .ThenInclude(x => x.Platforms)
                    .ThenInclude(x => x.Platform);
            }

            return query;
        }

        private IQueryable<CharacterPersona> ApplyPersonaFiltering(
            IQueryable<CharacterPersona> query,
            Guid characterId,
            List<Guid> personaIds)
        {
            query = query.Where(x => x.CharacterId == characterId && personaIds.Contains(x.PersonaId));

            return query;
        }

    }
}