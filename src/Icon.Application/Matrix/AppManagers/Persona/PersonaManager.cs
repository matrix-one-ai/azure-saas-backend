// using System;
// using System.Threading.Tasks;
// using Abp;
// using Abp.Domain.Repositories;
// using Abp.Domain.Services;
// using Abp.Domain.Uow;
// using Abp.Runtime.Session;


// namespace Icon.Matrix
// {
//     public interface IPersonaManager : IDomainService
//     {
//         Task<Guid> GetPersonaId(Guid platformId, string personaPlatformId, string personaName);
//         Task<Persona> GetPersona(Guid platformId, string platformPersonaId, string personaName);
//         //Task<Guid> GetCharacterPersonaId(Guid characterId, Guid personaId);
//     }
//     public class PersonaManager : IconServiceBase, IPersonaManager
//     {
//         private readonly IRepository<Persona, Guid> _personaRepository;
//         private readonly IRepository<PersonaPlatform, Guid> _personaPlatformRepository;
//         private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;
//         private readonly IUnitOfWorkManager _unitOfWorkManager;
//         private readonly IAbpSession _abpSession;

//         public PersonaManager(
//             IRepository<Persona, Guid> personaRepository,
//             IRepository<PersonaPlatform, Guid> personaPlatformRepository,
//             IRepository<CharacterPersona, Guid> personaCharacterRepository,
//             IUnitOfWorkManager unitOfWorkManager,
//             IAbpSession abpSession)
//         {
//             _personaRepository = personaRepository;
//             _personaPlatformRepository = personaPlatformRepository;
//             _characterPersonaRepository = personaCharacterRepository;
//             _unitOfWorkManager = unitOfWorkManager;
//             _abpSession = abpSession;
//         }

//         // public async Task<Guid> GetCharacterPersonaId(Guid characterId, Guid personaId)
//         // {
//         //     var characterPersona = await GetCharacterPersona(characterId, personaId);

//         //     return characterPersona.Id;
//         // }


//         // public async Task<CharacterPersona> GetCharacterPersona(
//         //     Guid characterId,
//         //     Guid personaId)
//         // {
//         //     var characterPersona = await _characterPersonaRepository.FirstOrDefaultAsync(p => p.CharacterId == characterId && p.PersonaId == personaId);

//         //     if (characterPersona == null)
//         //     {
//         //         characterPersona = await CreateCharacterPersona(characterId, personaId);
//         //     }

//         //     return characterPersona;
//         // }

//         // public async Task<CharacterPersona> CreateCharacterPersona(
//         //     Guid characterId,
//         //     Guid personaId)
//         // {
//         //     var characterPersona = new CharacterPersona
//         //     {
//         //         TenantId = (int)_abpSession.TenantId,
//         //         CharacterId = characterId,
//         //         PersonaId = personaId
//         //     };

//         //     using (var uow = _unitOfWorkManager.Begin())
//         //     {
//         //         characterPersona = await _characterPersonaRepository.InsertAsync(characterPersona);
//         //         await _unitOfWorkManager.Current.SaveChangesAsync();
//         //         await uow.CompleteAsync();
//         //     }

//         //     return characterPersona;
//         // }

//         public async Task<Guid> GetPersonaId(
//             Guid platformId,
//             string personaPlatformId,
//             string personaName)
//         {
//             var persona = await GetPersona(platformId, personaPlatformId, personaName);
//             return persona.Id;
//         }

//         public async Task<Persona> GetPersona(
//             Guid platformId,
//             string platformPersonaId,
//             string personaName)
//         {

//             var persona = await _personaRepository.FirstOrDefaultAsync(p => p.Id == personaPlatform.PersonaId);

//             if (Persona.Platforms == null)
//             {
//                 var personaPlatform = new PersonaPlatform
//                 {
//                     PersonaId = personaId,
//                     PlatformId = platformId,
//                     PlatformPersonaId = platformPersonaId
//                 };

//                 using (var uow = _unitOfWorkManager.Begin())
//                 {
//                     personaPlatform = await _personaPlatformRepository.InsertAsync(personaPlatform);
//                     await _unitOfWorkManager.Current.SaveChangesAsync();
//                     await uow.CompleteAsync();
//                 }
//             }

//             var personaPlatform = await _personaPlatformRepository.FirstOrDefaultAsync(p => p.PlatformId == platformId && p.PlatformPersonaId == platformPersonaId);

//             if (personaPlatform == null)
//             {
//                 personaPlatform = await CreatePersonaPlatform(platformId, platformPersonaId, personaName);
//             }



//             return persona;
//         }

//         private async Task<PersonaPlatform> CreatePersonaPlatform(
//             Guid platformId,
//             string platformPersonaId,
//             string personaName)
//         {
//             var Persona = new Persona
//             {
//                 TenantId = (int)_abpSession.TenantId,
//                 Name = personaName
//             };

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 Persona = await _personaRepository.InsertAsync(Persona);
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }

//             var personaPlatform = new PersonaPlatform
//             {
//                 TenantId = (int)_abpSession.TenantId,
//                 PersonaId = Persona.Id,
//                 PlatformId = platformId,
//                 PlatformPersonaId = platformPersonaId
//             };

//             using (var uow = _unitOfWorkManager.Begin())
//             {
//                 personaPlatform = await _personaPlatformRepository.InsertAsync(personaPlatform);
//                 await _unitOfWorkManager.Current.SaveChangesAsync();
//                 await uow.CompleteAsync();
//             }

//             return personaPlatform;
//         }

//     }
// }