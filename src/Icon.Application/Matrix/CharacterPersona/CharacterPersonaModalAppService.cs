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
using Icon.Matrix.Models;
using Icon.Matrix.CharacterPersonas.Forms;


namespace Icon.Matrix.CharacterPersonas
{
    [AbpAuthorize]
    public partial class CharacterPersonaAppService : IconAppServiceBase
    {
        [HttpPost]
        public async Task<BaseModalDto> GetModalNew(OpenBaseModalInput input)
        {
            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalNew, };
            if (!validModalTypes.Contains(input.ModalType)) throw new UserFriendlyException("Incorrect modal type");

            var platforms = await _platformRepository.GetAllListAsync();
            var characters = await _characterRepository.GetAllListAsync();
            var formModel = new CharacterPersonaFormModel(modalType: input.ModalType, platforms: platforms);
            formModel.SetMissingPlatforms(platforms);

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(CharacterPersona),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Persona);

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.Submit(
                    entity: nameof(CharacterPersona),
                    backendEvent: new BaseBackendEventDto(
                        backendServiceName : BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                        backendMethodName : BaseHelper.GetMethodName(nameof(Create))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            var form = new BaseFormDto(LocalizationManager);
            form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.CharacterPersonaCreate);
            form.Sections = new List<BaseFormSectionDto>
            {
                CharacterPersonaForm.GetSetupSection(),
                CharacterPersonaForm.GetCharacterSelectSection(characters.Select(c => new BaseFormDropdownOptionDto { Id = c.Id, Name = c.Name }).ToList()),
                CharacterPersonaForm.GetPersonaSection(),
                CharacterPersonaForm.GetPlatformsSection(),
            };

            form.SetFieldValues(formModel);
            modal.Form = form;

            await Task.CompletedTask;

            return modal;
        }

        // getmodalview
        [HttpPost]
        public async Task<BaseModalDto> GetModalView(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalView };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            var entity = await _characterManager.GetCharacterPersona(input.EntityId.Value);

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(CharacterPersona),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Persona);

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.SwitchToEditModal(
                    entity: nameof(CharacterPersona),
                    switchModalEvent: new BaseBackendEventDto(
                        modalType: BaseModalType.ModalEdit,
                        backendServiceName : BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                        backendMethodName : BaseHelper.GetMethodName(nameof(GetModalEdit))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.CharacterPersonaView);
            form.Sections = new List<BaseFormSectionDto>
            {
                CharacterPersonaForm.GetSetupSection(),
                CharacterPersonaForm.GetCharacterSection(),
                CharacterPersonaForm.GetPersonaSection().DisableAllFields(),
                CharacterPersonaForm.GetPlatformsSection().DisableAllFields(),
            };

            var formModel = new CharacterPersonaFormModel(entity);
            formModel.SetMissingPlatforms(await _platformRepository.GetAllListAsync());
            form.SetFieldValues(formModel);

            modal.Form = form;

            return modal;
        }


        [HttpPost]
        public async Task<BaseModalDto> GetModalEdit(OpenBaseModalInput input)
        {
            if (input.EntityId == null || input.EntityId == Guid.Empty)
                throw new UserFriendlyException("EntityId is required");

            var validModalTypes = new List<BaseModalType> { BaseModalType.ModalEdit };
            if (!validModalTypes.Contains(input.ModalType))
                throw new UserFriendlyException("Incorrect modal type");

            var entity = await _characterManager.GetCharacterPersona(input.EntityId.Value);

            var modal = new BaseModalBuilder().Build(
                modalType: input.ModalType,
                entityName: nameof(CharacterPersona),
                size: BaseModalSizeOptions.Large,
                icon: BaseIconOptions.Persona);

            modal.FooterButtons = new List<BaseModalFooterButtonDto>
            {
                BaseModalButtonFactory.Submit(
                    entity: nameof(CharacterPersona),
                    backendEvent: new BaseBackendEventDto(
                        backendServiceName: BaseHelper.GetServiceName(nameof(CharacterPersonaAppService)),
                        backendMethodName: BaseHelper.GetMethodName(nameof(Update))
                    )
                ),
                BaseModalButtonFactory.CloseModal
            };

            var form = new BaseFormBuilder(LocalizationManager).Build(formType: BaseFormType.CharacterPersonaEdit);
            form.Sections = new List<BaseFormSectionDto>
        {
            CharacterPersonaForm.GetSetupSection(),
            CharacterPersonaForm.GetCharacterSection(),
            CharacterPersonaForm.GetPersonaSection(),
            CharacterPersonaForm.GetPlatformsSection(),
        };

            var formModel = new CharacterPersonaFormModel(entity);
            formModel.SetMissingPlatforms(await _platformRepository.GetAllListAsync());
            form.SetFieldValues(formModel);

            modal.Form = form;

            return modal;
        }


        [HttpPost]
        public async Task<BaseModalSubmittedDto> Create(BaseModalDto modal)
        {
            if (modal == null) throw new UserFriendlyException("Modal is null");

            var formModel = new CharacterPersonaFormModel();
            modal.Form.UpdateDtoFromFieldValues(formModel);
            modal.Form.SetLocalization(LocalizationManager);
            modal.Form.ValidateRequiredFields();

            var entity = new CharacterPersona
            {
                Id = formModel.CharacterPersonaId,
                CharacterId = (Guid)formModel.Character.Id,
                Persona = new Models.Persona
                {
                    Id = formModel.Persona.Id,
                    Name = formModel.Persona.Name,
                    Platforms = new List<Models.PersonaPlatform>()
                }
            };

            UpdatePersonaPlatforms(entity, formModel);

            entity = await _characterManager.CreateCharacterPersona(entity);

            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.CreatedSuccesfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }

        [HttpPost]
        public async Task<BaseModalSubmittedDto> Update(BaseModalDto modal)
        {
            if (modal == null) throw new UserFriendlyException("Modal is null");

            var formModel = new CharacterPersonaFormModel();
            modal.Form.UpdateDtoFromFieldValues(formModel);



            var entity = await _characterManager.GetCharacterPersona(formModel.CharacterPersonaId);

            if (entity == null) throw new UserFriendlyException("Entity not found");

            entity.Persona.Name = formModel.Persona.Name;
            entity.Attitude = formModel.Attitude;
            entity.Repsonses = formModel.Responses;
            UpdatePersonaPlatforms(entity, formModel);

            await _characterManager.UpdateCharacterPersona(entity);

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

            await _characterManager.DeleteCharacterPersona(input.Id);

            return new BaseModalSubmittedDto
            {
                SuccessMessage = L("Modal.Submit.Message.DeletedSuccessfully"),
                RefreshListEvent = BaseListEvent.RefreshList
            };
        }


        private void UpdatePersonaPlatforms(CharacterPersona characterPersona, CharacterPersonaFormModel form)
        {
            if (characterPersona.Persona.Platforms == null)
            {
                characterPersona.Persona.Platforms = new List<Models.PersonaPlatform>();
            }

            UpdatePlatform(characterPersona, form.Twitter);
            UpdatePlatform(characterPersona, form.Discord);
            UpdatePlatform(characterPersona, form.Facebook);
            UpdatePlatform(characterPersona, form.Instagram);
            UpdatePlatform(characterPersona, form.Telegram);
        }

        private void UpdatePlatform(CharacterPersona characterPersona, Forms.PersonaPlatform platformFormModel)
        {
            if (characterPersona == null)
            {
                throw new UserFriendlyException("CharacterPersona is null");
            }

            if (platformFormModel == null)
            {
                throw new UserFriendlyException("PlatformFormModel is null");
            }

            var existingPlatform = characterPersona.Persona.Platforms.Where(p => p.PlatformId == platformFormModel.PlatformId).FirstOrDefault();



            if (existingPlatform == null)
            {
                characterPersona.Persona.Platforms.Add(new Models.PersonaPlatform
                {
                    PlatformId = platformFormModel.PlatformId,
                    PlatformPersonaId = platformFormModel.PlatformPersonaId
                });
            }
            else
            {
                existingPlatform.PlatformPersonaId = platformFormModel.PlatformPersonaId;
            }
        }

    }
}