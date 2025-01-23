using System;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Domain.Uow;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Azure.Data.Tables;
using Microsoft.Extensions.Configuration;
using Icon.Configuration;
using System.Collections.Generic;
using Icon.Matrix.Models;
using Icon.Matrix.Twitter;


namespace Icon.Matrix
{
    public interface ICharacterManager : IDomainService
    {
        Task<Guid> GetCharacterId(string name);
        Task<Character> GetCharacterById(Guid characterId);
        Task<Character> UpdateCharacter(Character character);
        Task<Guid> GetActiveCharacterBioId(Guid characterId);
        Task<CharacterBio> GetActiveCharacterBio(Guid characterId);
        Task<CharacterBio> CreateInitialCharacterBio(Guid characterId);

        Task<List<CharacterPersona>> GetCharacterPlatformPersonas(Guid characterId, Guid platformId);
        Task<Guid> GetCharacterPersonaId(Guid characterId, Guid platformId, string personaPlatformId, string personaName);
        Task<CharacterPersona> GetCharacterPersona(Guid characterPersonaId);
        Task<CharacterPersona> UpdateCharacterPersona(CharacterPersona characterPersona);
        Task<CharacterPersona> CreateCharacterPersona(CharacterPersona characterPersona);
        Task DeleteCharacterPersona(Guid characterPersonaId);
    }
    public class CharacterManager : IconServiceBase, ICharacterManager
    {
        private readonly IRepository<Character, Guid> _characterRepository;
        private readonly IRepository<CharacterBio, Guid> _characterBioRepository;
        private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;
        private readonly IRepository<Platform, Guid> _platformRepository;
        private readonly IRepository<PersonaPlatform, Guid> _personaPlatformRepository;
        private readonly IRepository<Persona, Guid> _personaRepository;

        private readonly ITwitterManager _twitterManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _abpSession;
        private IConfigurationRoot _configuration;


        public CharacterManager(
            IRepository<Character, Guid> characterRepository,
            IRepository<CharacterBio, Guid> characterBioRepository,
            IRepository<CharacterPersona, Guid> characterPersonaRepository,
            IRepository<Platform, Guid> platformRepository,
            IRepository<PersonaPlatform, Guid> personaPlatformRepository,
            IRepository<Persona, Guid> personaRepository,


            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession,
            IAppConfigurationAccessor appConfigurationAccessor)
        {
            _characterRepository = characterRepository;
            _characterBioRepository = characterBioRepository;
            _characterPersonaRepository = characterPersonaRepository;
            _platformRepository = platformRepository;
            _personaPlatformRepository = personaPlatformRepository;
            _personaRepository = personaRepository;

            _unitOfWorkManager = unitOfWorkManager;
            _abpSession = abpSession;
            _configuration = appConfigurationAccessor.Configuration;
        }

        public async Task<Guid> GetCharacterId(string name)
        {
            var character = await GetCharacter(name);
            return character.Id;
        }


        public async Task<Character> GetCharacterById(Guid characterId)
        {
            var character = await _characterRepository
                .GetAll()
                .Include(p => p.Bios)
                .FirstOrDefaultAsync(p => p.Id == characterId);

            if (character == null)
            {
                throw new UserFriendlyException("CharacterManager: Character not found");
            }

            if (character.Bios == null || character.Bios.Count == 0)
            {
                throw new UserFriendlyException("CharacterManager: Character does not have any bios");
            }

            return character;
        }

        public async Task<Character> UpdateCharacter(Character character)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                character = await _characterRepository.UpdateAsync(character);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            return character;
        }


        private async Task<Character> GetCharacter(string name)
        {

            var character = await _characterRepository.FirstOrDefaultAsync(p => p.Name == name);

            if (character == null)
            {
                character = await CreateCharacter(name);
            }

            return character;
        }

        private async Task<Character> CreateCharacter(string name)
        {
            var character = new Character
            {
                TenantId = (int)_abpSession.TenantId,
                Name = name
            };

            using (var uow = _unitOfWorkManager.Begin())
            {
                character = await _characterRepository.InsertAsync(character);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            return character;
        }

        public async Task<Guid> GetActiveCharacterBioId(Guid characterId)
        {
            var characterBio = await GetActiveCharacterBio(characterId);
            return characterBio.Id;
        }

        public async Task<CharacterBio> GetActiveCharacterBio(Guid characterId)
        {
            var characterBio = await _characterBioRepository
                .GetAll()
                .FirstOrDefaultAsync(p => p.CharacterId == characterId && p.IsActive);

            if (characterBio == null)
            {
                throw new UserFriendlyException("Character does not have an active bio");
            }

            // if (characterBio == null)
            // {
            //     characterBio = await CreateInitialCharacterBio(characterId);
            // }

            return characterBio;
        }

        public async Task<CharacterBio> CreateInitialCharacterBio(Guid characterId)
        {
            var characterBio = new CharacterBio
            {
                TenantId = (int)_abpSession.TenantId,
                CharacterId = characterId,
                ActiveFrom = DateTimeOffset.Now,
                IsActive = true
            };

            using (var uow = _unitOfWorkManager.Begin())
            {
                characterBio = await _characterBioRepository.InsertAsync(characterBio);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            return characterBio;
        }

        public async Task<List<CharacterPersona>> GetCharacterPlatformPersonas(Guid characterId, Guid platformId)
        {
            var characterPersonas = await _characterPersonaRepository
                .GetAll()
                .Include(p => p.Persona)
                .ThenInclude(p => p.Platforms)
                .ThenInclude(p => p.Platform)
                .Where(p => p.CharacterId == characterId)
                .ToListAsync();

            // filter only the personas that have the platform that we are interested in
            characterPersonas = characterPersonas.Where(p => p.Persona.Platforms.Any(p => p.PlatformId == platformId)).ToList();

            return characterPersonas;
        }

        public async Task<Guid> GetCharacterPersonaId(Guid characterId, Guid platformId, string personaPlatformId, string personaName)
        {
            var characterPersona = await GetCharacterPersona(characterId, platformId, personaPlatformId, personaName);
            return characterPersona.Id;
        }

        private async Task<CharacterPersona> GetCharacterPersona(
            Guid characterId,
            Guid platformId,
            string platformPersonaId,
            string personaName)
        {
            var characterPersona = await _characterPersonaRepository
                .GetAll()
                .Include(p => p.Persona)
                .ThenInclude(p => p.Platforms)
                .FirstOrDefaultAsync(p => p.CharacterId == characterId && p.Persona.Platforms.Any(p => p.PlatformId == platformId && p.PlatformPersonaId == platformPersonaId));

            if (characterPersona == null)
            {
                characterPersona = await CreateCharacterPersona(characterId, platformId, platformPersonaId, personaName);
            }

            return await GetCharacterPersona(characterPersona.Id);
        }

        public async Task<CharacterPersona> GetCharacterPersona(Guid characterPersonaId)
        {
            var characterPersona = await _characterPersonaRepository
                .GetAll()
                .Include(p => p.TwitterProfile)
                .Include(p => p.Character)

                .Include(p => p.Persona)
                .ThenInclude(p => p.Platforms)
                .ThenInclude(p => p.Platform)

                .FirstOrDefaultAsync(p => p.Id == characterPersonaId);

            return characterPersona;
        }

        private async Task<CharacterPersona> CreateCharacterPersona(
            Guid characterId,
            Guid platformId,
            string platformPersonaId,
            string personaName)
        {
            Guid characterPersonaId;

            using (var uow = _unitOfWorkManager.Begin())
            {
                var characterPersona = new CharacterPersona
                {
                    TenantId = (int)_abpSession.TenantId,
                    CharacterId = characterId,
                    Persona = new Persona
                    {
                        TenantId = (int)_abpSession.TenantId,
                        Name = personaName,
                        Platforms = new List<PersonaPlatform>()
                        {
                            new PersonaPlatform
                            {
                                TenantId = (int)_abpSession.TenantId,
                                PlatformId = platformId,
                                PlatformPersonaId = platformPersonaId
                            }
                        }
                    },
                    ShouldRespondNewPosts = false,
                    ShouldRespondMentions = true,
                    ShouldImportNewPosts = false
                };

                characterPersonaId = await _characterPersonaRepository.InsertAndGetIdAsync(characterPersona);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            return await GetCharacterPersona(characterPersonaId);
        }

        public async Task<CharacterPersona> CreateCharacterPersona(CharacterPersona characterPersona)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                characterPersona = await _characterPersonaRepository.InsertAsync(characterPersona);
                characterPersona = await GetCharacterPersona(characterPersona.Id);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            await AzureUpsertCharacterPersona(characterPersona);
            return characterPersona;
        }

        public async Task<CharacterPersona> UpdateCharacterPersona(CharacterPersona characterPersona)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                characterPersona = await _characterPersonaRepository.UpdateAsync(characterPersona);
                characterPersona = await GetCharacterPersona(characterPersona.Id);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }

            await AzureUpsertCharacterPersona(characterPersona);
            return characterPersona;
        }

        public async Task DeleteCharacterPersona(Guid characterPersonaId)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _characterPersonaRepository.DeleteAsync(characterPersonaId);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                await uow.CompleteAsync();
            }
        }

        public async Task AzureUpsertCharacterPersona(CharacterPersona characterPersona)
        {
            var connectionString = _configuration["AzureStorage:ConnectionString"];
            var twitterPlatform = await _platformRepository.FirstOrDefaultAsync(p => p.Name == "Twitter");
            var twitterPlatformId = twitterPlatform.Id;

            if (characterPersona.Persona == null)
            {
                throw new UserFriendlyException("Persona is null");
            }

            if (characterPersona.Persona.Platforms == null)
            {
                return;
            }

            var hasTwitter = characterPersona.Persona.Platforms.Any(p => p.PlatformId == twitterPlatformId);

            if (hasTwitter)
            {
                var personaPlatform = characterPersona.Persona.Platforms.FirstOrDefault(p => p.PlatformId == twitterPlatformId);
                var platformPersonaId = personaPlatform.PlatformPersonaId;

                TableClient tableClient = new TableClient(connectionString, "TwitterCharacterPersonas");
                TableEntity entity = new TableEntity(characterPersona.CharacterId.ToString(), platformPersonaId);
                entity["CharacterId"] = characterPersona.CharacterId;
                entity["CharacterName"] = characterPersona.Character.Name;
                entity["PlatformPersonaId"] = platformPersonaId;
                entity["PlatformPersonaName"] = characterPersona.Persona.Name;
                entity["ShouldRespondNewPosts"] = characterPersona.ShouldRespondNewPosts;
                entity["ShouldRespondMentions"] = characterPersona.ShouldRespondMentions;
                entity["ShouldImportNewPosts"] = characterPersona.ShouldImportNewPosts;
                entity["Timestamp"] = DateTime.UtcNow;

                await tableClient.UpsertEntityAsync(entity);
            }
        }

    }
}