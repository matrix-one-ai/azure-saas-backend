using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.UI;
using Microsoft.AspNetCore.Mvc;
using Icon.BaseManagement;
using Icon.Matrix.Models;
using Icon.Matrix.Characters.Forms;

namespace Icon.Matrix.Characters
{
    [AbpAuthorize]
    public partial class CharacterAppService : IconAppServiceBase
    {

        // -------------------------------------------------
        // GET MODAL NEW
        // -------------------------------------------------
        [HttpPost]
        public async Task<BaseModalDto> GetModalNew(OpenBaseModalInput input)
        {
            throw new UserFriendlyException("Not implemented");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalNew };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            // Build the modal
            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(Character),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Character // or whichever icon you prefer
            );

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.Submit(
                    entity: nameof(Character),
                    backendEvent: new BaseBackendEventDto(
                        backendServiceName: BaseHelper.GetServiceName(nameof(CharacterAppService)),
                        backendMethodName: BaseHelper.GetMethodName(nameof(Create))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            // Build the form
            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.CharacterCreate);
            form.Sections = new List<BaseFormSectionDto>
            {
                CharacterForm.GetSetupSection(),
                CharacterForm.GetMainSection(),
                CharacterForm.GetTwitterSection(),
                CharacterForm.GetPromptingSection(),
                CharacterForm.GetBioSection()
            };

            // Prepare the model (named CharacterPersonaFormModel in your code, but logically a "CharacterFormModel")
            var formModel = new CharacterFormModel
            {
                // If you need any defaults for a "New" scenario, set them here
                ModalType = BaseModalType.ModalNew
            };

            form.SetFieldValues(formModel);
            modal.Form = form;

            await Task.CompletedTask; // Just to match async signature
            return modal;
        }

        // -------------------------------------------------
        // GET MODAL VIEW
        // -------------------------------------------------
        [HttpPost]
        public async Task<BaseModalDto> GetModalView(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalView };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            var entity = await _characterManager.GetCharacterById(input.EntityId.Value);
            if (entity == null)
                throw new UserFriendlyException("Character not found");

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(Character),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Character
            );

            // Typical "View" modal might allow switching to edit
            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.SwitchToEditModal(
                    entity: nameof(Character),
                    switchModalEvent: new BaseBackendEventDto(
                        modalType: BaseModalType.ModalEdit,
                        backendServiceName: BaseHelper.GetServiceName(nameof(CharacterAppService)),
                        backendMethodName: BaseHelper.GetMethodName(nameof(GetModalEdit))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.CharacterView);
            form.Sections = new List<BaseFormSectionDto>
            {
                CharacterForm.GetSetupSection().DisableAllFields(),
                CharacterForm.GetMainSection().DisableAllFields(),
                CharacterForm.GetTwitterSection().DisableAllFields(),
                CharacterForm.GetPromptingSection().DisableAllFields(),
                CharacterForm.GetBioSection().DisableAllFields()
            };

            // Convert the retrieved entity to the form model
            var formModel = new CharacterFormModel(entity)
            {
                ModalType = BaseModalType.ModalView
            };

            form.SetFieldValues(formModel);
            modal.Form = form;

            return modal;
        }

        // -------------------------------------------------
        // GET MODAL EDIT
        // -------------------------------------------------
        [HttpPost]
        public async Task<BaseModalDto> GetModalEdit(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalEdit };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            var entity = await _characterManager.GetCharacterById(input.EntityId.Value);
            if (entity == null)
                throw new UserFriendlyException("Character not found");

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(Character),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Character
            );

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.Submit(
                    entity: nameof(Character),
                    backendEvent: new BaseBackendEventDto(
                        backendServiceName: BaseHelper.GetServiceName(nameof(CharacterAppService)),
                        backendMethodName: BaseHelper.GetMethodName(nameof(Update))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.CharacterEdit);
            form.Sections = new List<BaseFormSectionDto>
            {
                CharacterForm.GetSetupSection().DisableAllFields(),
                CharacterForm.GetMainSection().DisableAllFields(),
                CharacterForm.GetTwitterSection(),
                CharacterForm.GetPromptingSection(),
                CharacterForm.GetBioSection()
            };

            // Convert the entity to the form model
            var formModel = new CharacterFormModel(entity)
            {
                ModalType = BaseModalType.ModalEdit
            };

            form.SetFieldValues(formModel);
            modal.Form = form;

            return modal;
        }

        // -------------------------------------------------
        // CREATE
        // -------------------------------------------------
        [HttpPost]
        public async Task<BaseModalSubmittedDto> Create(BaseModalDto modal)
        {
            if (modal == null)
                throw new UserFriendlyException("Modal is null");

            var formModel = new CharacterFormModel();
            modal.Form.UpdateDtoFromFieldValues(formModel);
            modal.Form.SetLocalization(LocalizationManager);
            modal.Form.ValidateRequiredFields();

            var entity = new Character
            {
                Name = formModel.Name,

                // Fill out any other properties for new Character
                TwitterPostAgentId = formModel.TwitterPostAgentId,
                TwitterScrapeAgentId = formModel.TwitterScrapeAgentId,
                IsTwitterScrapingEnabled = formModel.IsTwitterScrapingEnabled,
                IsTwitterPostingEnabled = formModel.IsTwitterPostingEnabled,
                IsPromptingEnabled = formModel.IsPromptingEnabled,
                TwitterUserName = formModel.TwitterUserName
            };

            // If you store the "bio" directly or you have a separate entity for it,
            // set up the CurrentBio or related logic here:
            if (formModel.CurrentBio != null)
            {
                var newBio = new Models.CharacterBio// Your EF entity or model
                {
                    Bio = formModel.CurrentBio.Bio,
                    Personality = formModel.CurrentBio.Personality,
                    Appearance = formModel.CurrentBio.Appearance,
                    Occupation = formModel.CurrentBio.Occupation,
                    BirthDate = formModel.CurrentBio.BirthDate,
                    ActiveFrom = formModel.CurrentBio.ActiveFrom,
                    ActiveTo = formModel.CurrentBio.ActiveTo,
                    IsActive = formModel.CurrentBio.IsActive,
                    Motivations = formModel.CurrentBio.Motivations,
                    Fears = formModel.CurrentBio.Fears,
                    Values = formModel.CurrentBio.Values,
                    SpeechPatterns = formModel.CurrentBio.SpeechPatterns,
                    Skills = formModel.CurrentBio.Skills,
                    Backstory = formModel.CurrentBio.Backstory,
                    PublicPersona = formModel.CurrentBio.PublicPersona,
                    PrivateSelf = formModel.CurrentBio.PrivateSelf,
                    MediaPresence = formModel.CurrentBio.MediaPresence,
                    CrisisBehavior = formModel.CurrentBio.CrisisBehavior,
                    Relationships = formModel.CurrentBio.Relationships,
                    TechDetails = formModel.CurrentBio.TechDetails,
                };

                // Suppose Character has a collection of Bios
                entity.Bios = new List<Models.CharacterBio> { newBio };
            }

            // If you store prompt instructions or have them in a child entity, do that here:
            // e.g. entity.PromptInstructions = ...

            await Task.CompletedTask;
            //await _characterManager.CreateCharacter(entity);


            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.CreatedSuccesfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }

        // -------------------------------------------------
        // UPDATE
        // -------------------------------------------------
        [HttpPost]
        public async Task<BaseModalSubmittedDto> Update(BaseModalDto modal)
        {
            if (modal == null)
                throw new UserFriendlyException("Modal is null");

            var formModel = new CharacterFormModel();
            modal.Form.UpdateDtoFromFieldValues(formModel);

            var entity = await _characterManager.GetCharacterById(formModel.CharacterId);

            // If you don't have a "GetCharacterByNameAsync" method, adapt to suit your retrieval approach:
            // var entity = await _characterManager.GetCharacter(someGuidFrom the form);

            if (entity == null)
                throw new UserFriendlyException("Character not found");

            // Update basic properties
            entity.Name = formModel.Name;
            entity.TwitterUserName = formModel.TwitterUserName;
            entity.TwitterPostAgentId = formModel.TwitterPostAgentId;
            entity.TwitterScrapeAgentId = formModel.TwitterScrapeAgentId;
            entity.IsTwitterScrapingEnabled = formModel.IsTwitterScrapingEnabled;
            entity.IsTwitterPostingEnabled = formModel.IsTwitterPostingEnabled;
            entity.IsPromptingEnabled = formModel.IsPromptingEnabled;
            entity.PromptInstruction = formModel.PromptInstructions.PromptInstruction;
            entity.OutputExamples = formModel.PromptInstructions.OutputExamples;

            // Update current bio if needed:
            if (entity.Bios != null && entity.Bios.Any())
            {
                // Typically, you might only have one "active" bio. Find it:
                var activeBio = entity.Bios.FirstOrDefault(b => b.IsActive);
                if (activeBio != null && formModel.CurrentBio != null)
                {
                    activeBio.Bio = formModel.CurrentBio.Bio;
                    activeBio.Personality = formModel.CurrentBio.Personality;
                    activeBio.Appearance = formModel.CurrentBio.Appearance;
                    activeBio.Occupation = formModel.CurrentBio.Occupation;
                    activeBio.BirthDate = formModel.CurrentBio.BirthDate;

                    //activeBio.ActiveFrom = formModel.CurrentBio.ActiveFrom;
                    activeBio.ActiveTo = formModel.CurrentBio.ActiveTo;
                    activeBio.Motivations = formModel.CurrentBio.Motivations;
                    activeBio.Fears = formModel.CurrentBio.Fears;
                    activeBio.Values = formModel.CurrentBio.Values;
                    activeBio.SpeechPatterns = formModel.CurrentBio.SpeechPatterns;
                    activeBio.Skills = formModel.CurrentBio.Skills;
                    activeBio.Backstory = formModel.CurrentBio.Backstory;
                    activeBio.PublicPersona = formModel.CurrentBio.PublicPersona;
                    activeBio.PrivateSelf = formModel.CurrentBio.PrivateSelf;
                    activeBio.MediaPresence = formModel.CurrentBio.MediaPresence;
                    activeBio.CrisisBehavior = formModel.CurrentBio.CrisisBehavior;
                    activeBio.Relationships = formModel.CurrentBio.Relationships;
                    activeBio.TechDetails = formModel.CurrentBio.TechDetails;
                }
            }
            // Update prompt instructions if you store them somewhere

            await _characterManager.UpdateCharacter(entity);

            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.UpdatedSuccessfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }

        // -------------------------------------------------
        // DELETE
        // -------------------------------------------------
        [HttpPost]
        public async Task<BaseModalSubmittedDto> Delete(EntityDto<Guid> input)
        {
            if (input.Id == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            //await _characterManager.DeleteCharacter(input.Id);
            await Task.CompletedTask;

            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.DeletedSuccessfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }
    }
}
