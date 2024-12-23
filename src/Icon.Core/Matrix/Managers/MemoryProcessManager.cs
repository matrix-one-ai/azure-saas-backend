using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Icon.Matrix.AIManager.CharacterMentioned;
using Icon.Matrix.Enums;
using Icon.Matrix.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Icon.Matrix
{
    public partial interface IMemoryManager : IDomainService
    {
        Task<MemoryProcess> CreateMemoryProcess(Guid memoryId, bool reset = false);
        Task RunPendingProcesses(int batchSize = 10);
        Task RunProcess(Guid processId);
    }

    public partial class MemoryManager : IconServiceBase, IMemoryManager
    {
        public async Task<MemoryProcess> CreateMemoryProcess(Guid memoryId, bool reset = false)
        {
            var memory = await GetMemory(memoryId);

            if (memory == null)
                throw new Exception("Memory not found");

            if (memory.MemoryType == null)
                throw new Exception("Memory type not found");

            if (memory.Character == null)
                throw new Exception("Character not found");

            if (memory.MemoryProcess != null)
            {
                if (reset)
                {
                    await DeleteMemoryProcess(memory.MemoryProcess.Id);
                    await _memoryRepository.UpdateAsync(memory);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                }
                else
                {
                    return memory.MemoryProcess;
                }
            }

            if (memory.IsActionTaken == true)
            {
                return null;
            }

            var process = new MemoryProcess
            {
                TenantId = _tenantId,
                MemoryId = memory.Id,
                CreatedAt = DateTimeOffset.UtcNow,
                State = MemoryProcessState.NotStarted
            };

            using (var uow = _unitOfWorkManager.Begin())
            {
                process = await _memoryProcessRepository.InsertAsync(process);
                await _unitOfWorkManager.Current.SaveChangesAsync();

                var flow = MemoryProcessFlows.GetFlow(memory.MemoryType.Name);
                if (flow == null)
                {
                    process.State = MemoryProcessState.Completed;
                    process.CompletedAt = DateTimeOffset.UtcNow;
                    await _memoryProcessRepository.UpdateAsync(process);
                    await _unitOfWorkManager.Current.SaveChangesAsync();
                    uow.Complete();
                    return process;
                }

                int order = 0;
                foreach (var stepDef in flow.Steps)
                {
                    var stepEntity = new MemoryProcessStep
                    {
                        TenantId = _tenantId,
                        MemoryProcessId = process.Id,
                        OrderIndex = order++,
                        StepName = stepDef.StepName,
                        MethodName = stepDef.MethodName,
                        ParametersJson = stepDef.ParametersJson,
                        MaxRetries = stepDef.MaxRetries,
                        DependenciesJson = stepDef.Dependencies == null ? null : JsonConvert.SerializeObject(stepDef.Dependencies),
                        CreatedAt = DateTimeOffset.UtcNow
                    };
                    await _memoryProcessStepRepository.InsertAsync(stepEntity);
                }

                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }

            return process;
        }

        public async Task DeleteMemoryProcess(Guid processId)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _memoryProcessRepository.DeleteAsync(processId);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }
        }

        public async Task RunPendingProcesses(int batchSize = 10)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var processes = await _memoryProcessRepository.GetAll()
                    .Where(p => p.State == MemoryProcessState.NotStarted || p.State == MemoryProcessState.InProgress)
                    .OrderBy(p => p.CreatedAt)
                    .Take(batchSize)
                    .ToListAsync();

                foreach (var process in processes)
                {
                    await RunProcess(process.Id);
                }

                uow.Complete();
            }
        }

        public async Task RunProcess(Guid processId)
        {
            Memory memory;
            MemoryProcess process;

            var memoryId = await _memoryProcessRepository.GetAll()
                    .Include(p => p.Steps)
                    .Where(p => p.Id == processId)
                    .Select(p => p.MemoryId)
                    .FirstOrDefaultAsync();

            using (var uow = _unitOfWorkManager.Begin())
            {
                memory = await GetMemory(memoryId);
                process = memory.MemoryProcess;

                if (process == null)
                    return;

                if (process.State == MemoryProcessState.Completed || process.State == MemoryProcessState.Failed)
                    return; // already done

                process.State = MemoryProcessState.InProgress;
                process.StartedAt ??= DateTimeOffset.UtcNow;

                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }

            // Sort steps by order:
            var steps = process.Steps.OrderBy(s => s.OrderIndex).ToList();

            // Execute steps in order:
            foreach (var step in steps)
            {
                if (step.State == MemoryProcessStepState.Completed)
                    continue; // already completed

                // Check dependencies
                if (!await AreDependenciesMet(process, step))
                {
                    // Dependencies not met - skip for now. We don't fail or retry, just
                    // wait for next run. Another step might need to complete first.
                    await LogAsync(process.Id, step.Id, $"Skipping '{step.StepName}' because dependencies not met.", "Info");
                    continue;
                }

                var success = await ExecuteStepAsync(memory, process, step);

                if (!success && step.RetryCount >= step.MaxRetries)
                {
                    await LogAsync(process.Id, step.Id, "Step failed after max retries", "Error");
                    process.State = MemoryProcessState.Failed;
                    await UpdateProcessAsync(process);
                    return;
                }
                else if (!success)
                {
                    // Step failed but will retry next time
                    await UpdateProcessAsync(process);
                    return;
                }
            }

            // After trying all steps, check if all are complete:
            if (steps.All(s => s.State == MemoryProcessStepState.Completed))
            {
                process.State = MemoryProcessState.Completed;
                process.CompletedAt = DateTimeOffset.UtcNow;
                await UpdateProcessAsync(process);
            }
            else
            {
                // Not all steps are done, but no failed steps exceed max retries.
                // This might happen if dependencies not yet met. Just leave in InProgress.
                // Next RunProcess call will handle execution again.
                await UpdateProcessAsync(process);
            }
        }

        private async Task<bool> AreDependenciesMet(MemoryProcess process, MemoryProcessStep step)
        {
            if (string.IsNullOrEmpty(step.DependenciesJson))
                return true; // no dependencies

            var dependencies = JsonConvert.DeserializeObject<string[]>(step.DependenciesJson);
            if (dependencies == null || dependencies.Length == 0)
                return true;

            // All dependencies must be completed steps in the same process
            var completedSteps = process.Steps
                .Where(s => s.State == MemoryProcessStepState.Completed)
                .Select(s => s.StepName)
                .ToHashSet();

            return dependencies.All(d => completedSteps.Contains(d));
        }

        private async Task<bool> ExecuteStepAsync(Memory memory, MemoryProcess process, MemoryProcessStep step)
        {
            step.State = MemoryProcessStepState.InProgress;
            step.StartedAt = DateTimeOffset.UtcNow;
            await UpdateStepAsync(step);

            try
            {
                // Use reflection to find the method
                var method = this.GetType().GetMethod(step.MethodName, System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
                if (method == null)
                {
                    await LogAsync(process.Id, step.Id, $"Method '{step.MethodName}' not found.", "Error");
                    step.State = MemoryProcessStepState.Failed;
                    step.CompletedAt = DateTimeOffset.UtcNow;
                    await UpdateStepAsync(step);
                    return false; // structural error
                }

                // Prepare parameters
                var invokeParams = new object[] { memory, process, step.Id };

                var result = method.Invoke(this, invokeParams);

                if (result is Task<bool> boolTask)
                {
                    var success = await boolTask;
                    if (success)
                    {
                        step.State = MemoryProcessStepState.Completed;
                        step.CompletedAt = DateTimeOffset.UtcNow;
                        await UpdateStepAsync(step);
                        await LogAsync(process.Id, step.Id, $"Step '{step.StepName}' completed successfully.", "Info");
                        return true;
                    }
                    else
                    {
                        step.RetryCount++;
                        step.State = MemoryProcessStepState.Pending;
                        await UpdateStepAsync(step);
                        await LogAsync(process.Id, step.Id, $"Step '{step.StepName}' failed. Will retry.", "Warn");
                        return false;
                    }
                }
                else
                {
                    // If the method doesn't return Task<bool>, assume success
                    step.State = MemoryProcessStepState.Completed;
                    step.CompletedAt = DateTimeOffset.UtcNow;
                    await UpdateStepAsync(step);
                    await LogAsync(process.Id, step.Id, $"Step '{step.StepName}' completed successfully.", "Info");
                    return true;
                }
            }
            catch (Exception ex)
            {
                step.RetryCount++;
                step.State = MemoryProcessStepState.Pending;
                await UpdateStepAsync(step);
                await LogAsync(process.Id, step.Id, $"Exception in step '{step.StepName}': {ex.Message}", "Error", ex);
                return false;
            }
        }


        private async Task UpdateProcessAsync(MemoryProcess process)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _memoryProcessRepository.UpdateAsync(process);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }
        }

        private async Task UpdateStepAsync(MemoryProcessStep step)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _memoryProcessStepRepository.UpdateAsync(step);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }
        }

        private async Task LogAsync(
            Guid processId, Guid? stepId, string message, string level, Exception ex = null)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                var log = new MemoryProcessLog
                {
                    TenantId = _tenantId,
                    MemoryProcessId = processId,
                    MemoryProcessStepId = stepId,
                    Message = message,
                    LogLevel = level,
                    Exception = ex?.GetType().Name,
                    ExceptionMessage = ex?.Message,
                    LoggedAt = DateTimeOffset.UtcNow
                };

                await _memoryProcessLogRepository.InsertAsync(log);
                await _unitOfWorkManager.Current.SaveChangesAsync();
                uow.Complete();
            }
        }

        internal async Task<bool> ReplyMentionedTweetDetermineProcessWorkflow(Memory memory, MemoryProcess process, Guid stepId)
        {
            if (memory.CharacterPersona == null)
            {
                await LogAsync(process.Id, stepId, "I am missing a persona to determine the workflow", "Error");
                return false;
            }

            var personaName = memory.CharacterPersona.Persona.Name;
            await LogAsync(process.Id, stepId, $"Received a new twitter mention from {personaName}, determining what i will do.", "Thought");

            var shouldRespondMentions = memory.CharacterPersona.ShouldRespondMentions;
            if (!shouldRespondMentions)
            {
                await LogAsync(process.Id, stepId, $"I am not allowed to respond to mentions from {personaName}", "Thought");
                return false;
            }

            await LogAsync(process.Id, stepId, $"I know {personaName} and Matrix wants me to respond, great, I will respond!", "Thought");
            return true;
        }

        internal async Task<bool> ReplyMentionedTweetGatherContextStoredInDb(Memory memory, MemoryProcess process, Guid stepId)
        {

            await LogAsync(process.Id, stepId, "Gathering context from previous interactions to help me say something relevant...", "Thought");
            return true;
        }

        internal async Task<bool> ReplyMentionedTweetGatherContextFromUserProfile(Memory memory, MemoryProcess process, Guid stepId)
        {
            await LogAsync(process.Id, stepId, "Gathering context from user profile. Not Implemented", "Thought");
            return true;
        }

        internal async Task<bool> ReplyMentionedTweetGatherContextFromCurrencyAPI(Memory memory, MemoryProcess process, Guid stepId)
        {
            await LogAsync(process.Id, stepId, "Gathering context from currency API. Not Implemented", "Thought");
            return true;
        }

        internal async Task<bool> ReplyMentionedTweetGeneratePromptResponseToPost(Memory memory, MemoryProcess process, Guid stepId)
        {
            if (memory.IsPromptGenerated)
            {
                await LogAsync(process.Id, stepId, "Prompt already generated, skipping this step", "Thought");
                return true;
            }

            await LogAsync(process.Id, stepId, "Thinking about what i will respond... ", "Thought");
            if (memory.Character.IsPromptingEnabled)
            {
                var response = await RunCharacterMentionedPrompt(memoryId: memory.Id, AIModelType.Llama);

                if (response.IsSuccess)
                {
                    await LogAsync(process.Id, stepId, "I have generated a response to post", "Thought");

                    memory.IsPromptGenerated = true;
                    await UpdateMemory(memory);

                    return true;
                }
                else
                {
                    await LogAsync(process.Id, stepId, "I have failed to generate a response to post", "Thought");
                    return false;
                }
            }
            else
            {
                await LogAsync(process.Id, stepId, $"Ouch, I am not allowed to generate prompts for character {memory.Character.Name}", "Thought");
                return false;
            }
        }

        internal async Task<bool> ReplyMentionedPostTwitterReply(Memory memory, MemoryProcess process, Guid stepId)
        {
            if (memory.IsActionTaken)
            {
                await LogAsync(process.Id, stepId, "Tweet already replied, skipping this step", "Thought");
                return true;
            }

            await LogAsync(process.Id, stepId, "Posting response to twitter", "Thought");

            if (!memory.Character.IsTwitterPostingEnabled)
            {
                await LogAsync(process.Id, stepId, "I am not allowed to post, skipping this step", "Thought");
                return false;
            }

            if (memory.Character.TwitterPostAgentId == null)
            {
                await LogAsync(process.Id, stepId, "I am missing an agent to post with!", "Error");
                return false;
            }

            var tweetId = memory.PlatformInteractionId;
            var prompt = memory.Prompts?.OrderByDescending(p => p.GeneratedAt)?.FirstOrDefault();

            if (tweetId == null)
            {
                await LogAsync(process.Id, stepId, "TweetId is null, cant reply to tweet", "Thought");
                return false;
            }

            if (prompt == null)
            {
                await LogAsync(process.Id, stepId, "Prompt is null, cant reply to tweet", "Thought");
                return false;
            }

            var promptJson = prompt.ResponseJson;
            AICharacterMentionedResponse promptResponse = null;
            if (promptJson != null)
            {
                try
                {
                    promptResponse = JsonConvert.DeserializeObject<AICharacterMentionedResponse>(promptJson);
                }
                catch (Exception e)
                {
                    await LogAsync(process.Id, stepId, "Failed to deserialize prompt response", "Error", e);
                    return false;
                }
            }

            var tweetContent = promptResponse.ResultToPost;
            if (string.IsNullOrWhiteSpace(tweetContent))
            {
                await LogAsync(process.Id, stepId, "Prompt response is empty, cant reply to tweet", "Thought");
                return false;
            }

            await LogAsync(process.Id, stepId, $"Replying mention with: {tweetContent}", "Thought");

            try
            {
                var replyTweet = await _twitterManager.ReplyToTweetAsync(memory.Character, tweetId, tweetContent);

                memory.IsActionTaken = true;
                await UpdateMemory(memory);

                await StoreCharacterReplyTweets(
                    characterId: memory.CharacterId,
                    conversationId: memory.PlatformInteractionParentId,
                    tweetId: replyTweet.Data?.Id,
                    tweetContent: tweetContent
                );

                await LogAsync(process.Id, stepId, "Reply posted successfully", "Info");
                return true;
            }
            catch (Exception e)
            {
                await LogAsync(process.Id, stepId, "Failed to post reply", "Error", e);
                return false;
            }
        }
    }
}

