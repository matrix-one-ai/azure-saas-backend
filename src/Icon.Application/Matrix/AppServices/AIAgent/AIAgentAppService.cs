// using System.Collections.Generic;
// using Abp.Domain.Repositories;
// using System.Linq;
// using System.Threading.Tasks;
// using Abp;
// using Abp.Application.Services.Dto;
// using Abp.Authorization;
// using Abp.Linq.Extensions;
// using Microsoft.EntityFrameworkCore;
// using System;
// using Abp.UI;
// using Icon.Matrix.Portal.Dto;
// using Icon.Matrix.Models;
// using Microsoft.AspNetCore.Mvc;
// using Abp.Collections.Extensions;
// using System.Linq.Dynamic.Core;
// using Icon.Matrix.AIManager;
// using OpenAI.Models;
// using Icon.Matrix.Enums;

// namespace Icon.Matrix
// {
//     [AbpAuthorize]
//     public class AIAgentAppService : IconAppServiceBase
//     {
//         private readonly IRepository<Character, Guid> _characterRepository;
//         private readonly IRepository<CharacterPersona, Guid> _characterPersonaRepository;
//         private readonly IAIManager _aiManager;

//         public AIAgentAppService(
//             IRepository<Character, Guid> characterRepository,
//             IRepository<CharacterPersona, Guid> characterPersonaRepository,
//             IAIManager aiManager)
//         {
//             _characterRepository = characterRepository;
//             _characterPersonaRepository = characterPersonaRepository;
//             _aiManager = aiManager;
//         }

//         [HttpPost]
//         public async Task<string> GetAgentResponse(string prompt)
//         {
//             var aiModel = AIModelType.OpenAI;
//             var response = await _aiManager.GenerateResponse(prompt, aiModel);
//             return response;
//         }
//     }
// }