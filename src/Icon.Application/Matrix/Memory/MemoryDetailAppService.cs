// using System.Collections.Generic;
// using Abp.Domain.Repositories;
// using Icon.Chat.Dto;
// using System.Linq;
// using System.Threading.Tasks;
// using Abp;
// using Abp.Application.Services.Dto;
// using Abp.Auditing;
// using Abp.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using Microsoft.EntityFrameworkCore;
// using System.Linq.Dynamic.Core;
// using System;
// using Icon.Matrix.AIManager.CharacterMentioned;
// using Icon.Matrix.AIManager;
// using Abp.UI;
// using Icon.Matrix.Enums;
// using Icon.Matrix.Models;
// using Icon.Matrix.Portal.Dto;

// namespace Icon.Matrix.Memories
// {
//     // 01 Reply to user mention

//     // characterBIO 
//     // current post to respond to 
//     // last post that we responded to for the user
//     // userprofile of the user to respond to
//     // the [conversation] that we responding to 
//     // the bio of the user that is talking to us (twitterprofile) - if we have it
//     // if we attitude add the attitude of the user that is talking to us
//     // if we have examples responses for the user
//     // most 10 recent post of the user that is talking to us
//     // the current ranking of the user
//     // the statistics of the user that is talking to us - amount of tweets etc

//     // new mention ... azt .. memory .. build context --> send context to Prompt API --> [receive context / handle context] --> get response --> store pot response
//     // potention response to the user --> action --> post the response to twitter.


//     // 02 Create a new tweet 
//     // 03 Comment on a timelinepost


//     [AbpAuthorize]
//     public partial class MemoryAppService : IconAppServiceBase
//     {
//         [HttpGet]
//         public async Task<AICharacterMentionedContext> GenerateMentionedPromptContext(Guid memoryId)
//         {
//             AbpSession.Use(2, 3);

//             var context = await _memoryManager.GetPromptContext(memoryId, AIPromptType.ReplyMentionedTweet);
//             return context;
//         }

//         [HttpGet]
//         public async Task<string> GetPromptInput(Guid memoryId)
//         {
//             AbpSession.Use(2, 3);
//             return await _memoryManager.GetPromptInput(memoryId, AIPromptType.ReplyMentionedTweet);
//         }


//         [HttpGet]
//         public async Task<MemoryPrompt> GeneratePromptResponse(Guid memoryId)
//         {
//             AbpSession.Use(2, 3);
//             var response = await _memoryManager.GeneratePromptResponse(
//                 memoryId,
//                 AIPromptType.ReplyMentionedTweet,
//                 AIModelType.OpenAI);

//             return response;
//         }

//         [HttpGet]
//         public async Task<MemoryProcess> CreateMemoryProcess(Guid memoryId, bool reset = true)
//         {
//             AbpSession.Use(2, 3);
//             var process = await _memoryManager.CreateMemoryProcess(memoryId, reset);
//             return process;
//         }

//         [HttpGet]
//         public async Task RunPendingProcesses(int batchSize = 10)
//         {
//             AbpSession.Use(2, 3);
//             await _memoryManager.RunPendingProcesses(batchSize);
//         }

//         [HttpGet]
//         public async Task<List<MemoryProcessDto>> GetMemoryProcesses()
//         {
//             AbpSession.Use(2, 3);
//             var memories = await _memoryRepository.GetAll()
//                 .Include(x => x.MemoryProcess)
//                     .ThenInclude(x => x.Steps)

//                 .Include(x => x.MemoryProcess)
//                     .ThenInclude(x => x.Logs)

//                 .Where(x => x.MemoryProcess != null)
//                 .OrderByDescending(x => x.MemoryProcess.CreatedAt)
//                 .ToListAsync();

//             return ObjectMapper.Map<List<MemoryProcessDto>>(memories.Select(x => x.MemoryProcess));
//         }

//         //MemoryThoughtDto

//         // [HttpGet]
//         // public async Task<List<MemoryThoughtDto>> GetMemoryThoughts()
//         // {
//         //     AbpSession.Use(2, 3);
//         //     var memories = await _memoryRepository.GetAll()
//         //         .Include(x => x.MemoryProcess)
//         //             .ThenInclude(x => x.Steps)

//         //         .Include(x => x.MemoryProcess)
//         //             .ThenInclude(x => x.Logs)

//         //         .Where(x => x.MemoryProcess != null)
//         //         .OrderByDescending(x => x.MemoryProcess.CreatedAt)
//         //         .ToListAsync();

//         //     var thoughts =
//         //         memories.SelectMany(x => x.MemoryProcess.Se
//         //     new List<MemoryThoughtDto>();
//         // }

//     }
// }