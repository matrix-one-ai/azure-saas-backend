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
using Icon.Matrix.Models;

namespace Icon.Matrix
{
    [AbpAuthorize]
    public class PersonaAppService : IconAppServiceBase
    {
        private readonly IRepository<Persona, Guid> _personaRepository;

        public PersonaAppService(
            IRepository<Persona, Guid> personaRepository)
        {
            _personaRepository = personaRepository;
        }

        [HttpPost]
        public async Task<PagedResultDto<PersonaDto>> GetPersonas(GetPersonasInput input)
        {
            var query = GetPersonasQuery(withProperties: true);
            var filteredQuery = ApplyFiltering(query, input);

            var queryCount = await GetPersonaCount(filteredQuery);
            var memories = await filteredQuery
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var mappedResult = ObjectMapper.Map<List<PersonaDto>>(memories);
            mappedResult = ApplyRowSettings(mappedResult);

            return new PagedResultDto<PersonaDto>
            {
                TotalCount = queryCount,
                Items = mappedResult
            };
        }

        private IQueryable<Persona> GetPersonasQuery(bool withProperties)
        {
            var query = _personaRepository.GetAll().AsNoTracking();

            // if (withProperties)
            // {
            //     query = query;
            // }

            return query;
        }

        private async Task<int> GetPersonaCount(IQueryable<Persona> query)
        {
            var queryCount = await query.CountAsync();
            return queryCount;
        }

        private IQueryable<Persona> ApplyFiltering(IQueryable<Persona> query, GetPersonasInput input)
        {
            // query = query
            //     .WhereIf(!input.PersonaPersonaFilter.IsNullOrEmpty(),
            //         x => x.CharacterPersona.Persona.Name.Contains(input.PersonaPersonaFilter))
            //     .WhereIf(!input.PersonaContentFilter.IsNullOrEmpty(),
            //         x => x.PersonaContent.Contains(input.PersonaContentFilter) || x.PersonaTitle.Contains(input.PersonaContentFilter))
            //     .WhereIf(input.DateTimeStart.HasValue && input.DateTimeEnd.HasValue,
            //         x => x.CreatedAt >= input.DateTimeStart && x.CreatedAt <= input.DateTimeEnd);

            return query;
        }

        private List<PersonaDto> ApplyRowSettings(List<PersonaDto> memories)
        {
            foreach (var persona in memories)
            {
                // persona.RowSettings = new BaseManagement.BaseListRowSettingsDto
                // {
                //     CanOpen = true,
                //     CanEdit = true,
                // };
            }

            return memories;
        }

        // public async Task<PersonaDto> CreatePersona(CreatePersonaInput input)
        // {
        //     var platformId = _personaRepository.GetAll().Where(p => p.Name == input.PlatformName).Select(p => p.Id).FirstOrDefault();
        //     if(platformId == Guid.Empty)
        //     {
        //         var platform = new Persona
        //         {
        //             Name = input.PlatformName
        //         };
        //         platform = await _personaRepository.InsertAsync(platform);
        //         platformId = platform.Id;
        //     }
        // }

        // public async Task<CharacterBioDto> CreateCharacterBio(CreateCharacterBioInput input)
        // {
        //     var characterBio = ObjectMapper.Map<CharacterBio>(input);
        //     characterBio.TenantId = AbpSession.TenantId.Value;
        //     characterBio = await _characterBioRepository.InsertAsync(characterBio);

        //     return ObjectMapper.Map<CharacterBioDto>(characterBio);
        // }




    }
}