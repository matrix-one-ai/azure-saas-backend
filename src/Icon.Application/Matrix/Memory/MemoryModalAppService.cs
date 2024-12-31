using System.Collections.Generic;
using Abp.Domain.Repositories;
using Icon.Chat.Dto;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Auditing;
using Abp.Authorization;
using System;
using Microsoft.AspNetCore.Mvc;
using Icon.BaseManagement;
using Abp.UI;
using Icon.Matrix.Memories.Forms;
using Icon.Matrix.Enums;
using Icon.Matrix.Twitter;
using Newtonsoft.Json;
using Icon.Matrix.AIManager.CharacterMentioned;

namespace Icon.Matrix.Memories
{
    [AbpAuthorize]
    public partial class MemoryAppService : IconAppServiceBase
    {
        [HttpPost]
        public async Task<BaseModalDto> GetModalView(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalView };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            var entity = await _memoryManager.GetMemory(input.EntityId.Value);

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(Models.Memory),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Persona);

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.SwitchToEditModal(
                    entity: nameof(Models.Memory),
                    switchModalEvent: new BaseBackendEventDto(
                        modalType: BaseModalType.ModalEdit,
                        backendServiceName : BaseHelper.GetServiceName(nameof(MemoryAppService)),
                        backendMethodName : BaseHelper.GetMethodName(nameof(GetModalEdit))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.MemoryView);
            form.Sections = new List<BaseFormSectionDto>
            {
                MemoryForm.GetSetupSection(),
                MemoryForm.GetCharacterSection(),
                MemoryForm.GetPersonaSection().DisableAllFields(),
                MemoryForm.GetMemorySection().DisableAllFields(),
                MemoryForm.GetPromptSection().DisableAllFields()
            };

            var formModel = new MemoryFormModel(entity);
            form.SetFieldValues(formModel);

            modal.Form = form;
            modal.PrompResponseJson = entity.Prompts?.OrderBy(p => p.GeneratedAt);

            return modal;
        }

        [HttpPost]
        public async Task<BaseModalDto> GeneratePrompt(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            await _memoryManager.RunCharacterMentionedPrompt(
                memoryId: input.EntityId.Value,
                modelType: AIModelType.DirectOpenAI
            );

            return await GetModalEdit(input);
        }

        [HttpPost]
        public async Task<BaseModalDto> PostTweet(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var memory = await _memoryManager.GetMemory(input.EntityId.Value);
            var character = memory.Character;
            var agentId = character.TwitterPostAgentId;
            var prompt = memory.Prompts?.OrderByDescending(p => p.GeneratedAt)?.FirstOrDefault();

            if (character.IsTwitterPostingEnabled == false)
                throw new UserFriendlyException($"Character {character.Name} using agent {character.TwitterPostAgentId} is not enabled for posting tweets");

            if (prompt == null)
                throw new UserFriendlyException("Prompt not found");

            if (prompt.IsSuccess == false)
                throw new UserFriendlyException("Last Prompt is not successful");

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
                    throw new UserFriendlyException("Prompt response is not valid or could not be deserialized");
                }
            }

            var tweetIdToReplyTo = memory.PlatformInteractionId;
            var tweetContent = promptResponse.ResultToPost;

            try
            {
                var tweet = await _twitterManager.ReplyToTweetAsync(character, tweetIdToReplyTo, tweetContent);
                memory.IsActionTaken = true;
                await _memoryManager.UpdateMemory(memory);

                await _memoryManager.StoreCharacterReplyTweets(
                    characterId: memory.CharacterId,
                    conversationId: memory.PlatformInteractionParentId,
                    tweetId: tweet.Data.Id,
                    tweetContent: tweetContent
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException("Error posting tweet: " + ex.Message);
            }

            return await GetModalEdit(input);
        }


        [HttpPost]
        public async Task<BaseModalDto> GetModalEdit(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalEdit };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            var entity = await _memoryManager.GetMemory(input.EntityId.Value);

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(Models.Memory),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Persona);

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.Submit(
                    entity: nameof(Models.Memory),
                    backendEvent: new BaseBackendEventDto(
                        backendServiceName: BaseHelper.GetServiceName(nameof(MemoryAppService)),
                        backendMethodName: BaseHelper.GetMethodName(nameof(Update))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };
            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.MemoryEdit);
            form.Sections = new List<BaseFormSectionDto>
            {
                MemoryForm.GetSetupSection(),
                MemoryForm.GetCharacterSection(),
                MemoryForm.GetPersonaSection().DisableAllFields(),
                MemoryForm.GetMemorySection().DisableAllFields(),
            };

            if (entity.MemoryType.Name == "CharacterMentionedTweet")
            {
                modal.FooterButtons.Add(BaseModalButtonFactory.GeneratePrompt(
                    switchModalEvent: new BaseBackendEventDto(
                        modalType: BaseModalType.ModalEdit,
                        backendServiceName: BaseHelper.GetServiceName(nameof(MemoryAppService)),
                        backendMethodName: BaseHelper.GetMethodName(nameof(GeneratePrompt))
                    )
                ));

                if (entity.Prompts?.OrderBy(p => p.GeneratedAt).LastOrDefault()?.IsSuccess == true)
                {
                    form.Sections.Add(MemoryForm.GetPromptSection().DisableAllFields());

                    modal.FooterButtons.Add(BaseModalButtonFactory.PostTweet(
                        switchModalEvent: new BaseBackendEventDto(
                            modalType: BaseModalType.ModalEdit,
                            backendServiceName: BaseHelper.GetServiceName(nameof(MemoryAppService)),
                            backendMethodName: BaseHelper.GetMethodName(nameof(PostTweet))
                        )
                    ));
                }
            }

            var formModel = new MemoryFormModel(entity);
            form.SetFieldValues(formModel);

            modal.Form = form;
            modal.PrompResponseJson = entity.Prompts?.OrderByDescending(p => p.GeneratedAt);

            return modal;
        }


        [HttpPost]
        public async Task<BaseModalSubmittedDto> Update(BaseModalDto modal)
        {
            if (modal == null) throw new UserFriendlyException("Modal is null");

            var formModel = new MemoryFormModel();
            modal.Form.UpdateDtoFromFieldValues(formModel);

            var entity = await _memoryManager.GetMemory(formModel.MemoryId);

            if (entity == null) throw new UserFriendlyException("Entity not found");

            // entity.Persona.Name = formModel.Persona.Name;
            // entity.Attitude = formModel.Attitude;
            // entity.Repsonses = formModel.Responses;


            await _memoryManager.UpdateMemory(entity);

            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.UpdatedSuccessfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }

        [HttpPost]
        public async Task<BaseModalSubmittedDto> Delete(EntityDto<Guid> input)
        {
            if (input.Id == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            await _memoryManager.DeleteMemory(input.Id);

            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.DeletedSuccessfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }



    }
}